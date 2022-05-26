using Fries.Api.Attributes;
using Fries.Helpers;
using System.Net;

namespace Fries.Api.Middlewares
{
    public class SafeListMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<SafeListMiddleware> _logger;
        private readonly byte[][] _safelist;

        public SafeListMiddleware(RequestDelegate next, ILogger<SafeListMiddleware> logger, IEnumerable<string> safelist)
        {
            var ips = safelist.ToArray();
            _safelist = new byte[safelist.Count()][];
            for (var i = 0; i < ips.Length; i++)
            {
                _safelist[i] = IPAddress.Parse(ips[i]).GetAddressBytes();
            }

            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            // check AllowAnonymousIp attribute
            var isAllowAnonymousIp = false;

            var endpoint = context.GetEndpoint();
            if (endpoint != null)
            {
                isAllowAnonymousIp = endpoint.Metadata.Any(x => x.GetType() == typeof(AllowAnonymousIpAttribute));
            }

            if (!isAllowAnonymousIp)
            {
                var remoteIp = context.Connection.RemoteIpAddress;

                var bytes = remoteIp?.GetAddressBytes() ?? Array.Empty<byte>();
                var badIp = true;
                foreach (var address in _safelist)
                {
                    if (address.SequenceEqual(bytes))
                    {
                        badIp = false;
                        break;
                    }
                }

                if (badIp)
                {
                    _logger.LogWarning(CustomException.Authentication.ForbiddenIpAddress(remoteIp?.ToString() ?? string.Empty), string.Empty);
                    context.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                    return;
                }
            }

            await _next.Invoke(context);
        }
    }
}
