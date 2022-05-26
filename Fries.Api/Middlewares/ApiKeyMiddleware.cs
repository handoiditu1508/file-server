using Fries.Api.Attributes;
using Fries.Helpers;
using Fries.Helpers.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace Fries.Api.Middlewares
{
    public class ApiKeyMiddleware
    {
        private readonly RequestDelegate _next;

        public ApiKeyMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var endpoint = context.GetEndpoint();

            if (endpoint == null)
            {
                await _next(context);
                return;
            }

            // check ApiKeyAuthenticateAttribute attribute
            var isApiKeyProtected = endpoint.Metadata.Any(x => x.GetType() == typeof(ApiKeyAuthenticateAttribute));
            if (!isApiKeyProtected)
            {
                await _next(context);
                return;
            }

            // check AllowAnonymous attribute
            var isAllowAnonymous = endpoint.Metadata.Any(x => x.GetType() == typeof(AllowAnonymousAttribute));
            if (isAllowAnonymous)
            {
                await _next(context);
                return;
            }

            // check Api key exist in header
            if (!context.Request.Headers.TryGetValue(AppSettings.ApiKey.Name, out var extractedApiKey))
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;

                var response = CustomException.Authentication.ApiKeyNotFound.ToSimpleError();

                // Get the options
                var jsonOptions = context.RequestServices.GetService<IOptions<JsonOptions>>();
                var jsonResponse = JsonSerializer.Serialize(response, jsonOptions?.Value.SerializerOptions);

                await context.Response.WriteAsync(jsonResponse);
                return;
            }

            // check Api key value
            if (!AppSettings.ApiKey.Value.Equals(extractedApiKey))
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;

                var response = CustomException.Authentication.InvalidApiKey.ToSimpleError();

                // Get the options
                var jsonOptions = context.RequestServices.GetService<IOptions<JsonOptions>>();
                var jsonResponse = JsonSerializer.Serialize(response, jsonOptions?.Value.SerializerOptions);

                await context.Response.WriteAsync(jsonResponse);
                return;
            }

            await _next(context);
        }
    }
}
