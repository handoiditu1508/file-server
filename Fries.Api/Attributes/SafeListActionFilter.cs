using Fries.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Net;

namespace Fries.Api.Attributes
{
    // use [ServiceFilter(typeof(ClientIpCheckActionFilter))] decorator to apply this attribute
    public class SafeListActionFilter : ActionFilterAttribute
    {
        private readonly ILogger<SafeListActionFilter> _logger;
        private readonly string[] _safelist;

        public SafeListActionFilter(IEnumerable<string> safelist, ILogger<SafeListActionFilter> logger)
        {
            _safelist = safelist.ToArray();
            _logger = logger;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            // check AllowAnonymousIp attribute
            var isAllowAnonymousIp = false;

            var endpoint = context.HttpContext.GetEndpoint();
            if (endpoint != null)
            {
                isAllowAnonymousIp = endpoint.Metadata.Any(x => x.GetType() == typeof(AllowAnonymousIpAttribute));
            }

            if (!isAllowAnonymousIp)
            {
                var remoteIp = context.HttpContext.Connection.RemoteIpAddress;
                var badIp = true;

                if (remoteIp.IsIPv4MappedToIPv6)
                {
                    remoteIp = remoteIp.MapToIPv4();
                }

                foreach (var address in _safelist)
                {
                    var testIp = IPAddress.Parse(address);

                    if (testIp.Equals(remoteIp))
                    {
                        badIp = false;
                        break;
                    }
                }

                if (badIp)
                {
                    _logger.LogWarning(CustomException.Authentication.ForbiddenIpAddress(remoteIp?.ToString() ?? string.Empty), string.Empty);
                    context.Result = new StatusCodeResult(StatusCodes.Status403Forbidden);
                    return;
                }
            }

            base.OnActionExecuting(context);
        }
    }

    public static class SafeListActionFilterExtension
    {
        public static void AddSafeListActionFilter(this IServiceCollection services)
        {
            services.AddScoped(container =>
            {
                var loggerFactory = container.GetRequiredService<ILoggerFactory>();
                var logger = loggerFactory.CreateLogger<SafeListActionFilter>();

                return new SafeListActionFilter(AppSettings.SafeList, logger);
            });
        }
    }
}
