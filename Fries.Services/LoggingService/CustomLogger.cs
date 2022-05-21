﻿using Fries.Helpers;
using Fries.Helpers.Abstractions;
using Fries.Helpers.Extensions;
using Microsoft.Extensions.Logging;
using System.Runtime.Versioning;

namespace Fries.Services.LoggingService
{
    public class CustomLogger : ILogger
    {
        private readonly string _categoryName;
        private readonly IHttpHelper _httpHelper;

        public CustomLogger(string categoryName)
        {
            _categoryName = categoryName;
            _httpHelper = new HttpHelper();
            _httpHelper.SetBaseUrl(AppSettings.LoggingService.BaseUrl);
        }

        public IDisposable BeginScope<TState>(TState state) => default!;

        public bool IsEnabled(LogLevel logLevel) => true;

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
        {
            try
            {
                if (exception == null)
                {
                    var message = formatter(state, exception);
                    exception = logLevel > LogLevel.Information ? CustomException.System.UnexpectedError(message) : CustomException.System.Notification(message);
                }
                _httpHelper.Post("api/MongoLogging/InsertLog", exception.ToInsertLogRequest(logLevel));
            }
            catch
            { }
        }
    }

    [UnsupportedOSPlatform("browser")]
    [ProviderAlias("CustomLogger")]
    public sealed class CustomLoggerProvider : ILoggerProvider
    {
        public CustomLoggerProvider()
        {
        }

        public ILogger CreateLogger(string categoryName)
        {
            return new CustomLogger(categoryName);
        }

        public void Dispose()
        {
        }
    }
}
