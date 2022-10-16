using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NuGetLogger = global::NuGet.Common.ILogger;
using MicrosoftLogger = Microsoft.Extensions.Logging.ILogger;

namespace JustinWritesCode.NuGetUtils
{
    public class LoggerWrapper : NuGetLogger, MicrosoftLogger
    {
        MicrosoftLogger _logger;

        public LoggerWrapper(MicrosoftLogger logger)
        {
            _logger = logger;
        }

        public virtual IDisposable BeginScope<TState>(TState state)
        {
            return _logger.BeginScope(state);
        }

        public virtual bool IsEnabled(LogLevel logLevel)
        {
            return _logger.IsEnabled(logLevel);
        }

        public virtual void Log(global::NuGet.Common.ILogMessage message)
        {
            _logger.Log((LogLevel)message.Level, message.Message);
        }

        public virtual void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
        {
            _logger.Log(logLevel, eventId, state, exception, formatter);
        }

        public virtual void Log(global::NuGet.Common.LogLevel level, string data)
        {
            _logger.Log((LogLevel)level, data);
        }

        public virtual async Task LogAsync(global::NuGet.Common.LogLevel level, string data)
        {
            _logger.Log((LogLevel)level, data);
        }

        public async Task LogAsync(global::NuGet.Common.ILogMessage message)
        {
            _logger.Log((LogLevel)message.Level, message.Message);
        }

        public virtual void LogDebug(string data)
        {
            throw new NotImplementedException();
        }

        public virtual void LogError(string data)
        {
            throw new NotImplementedException();
        }

        public virtual void LogInformation(string data)
        {
            throw new NotImplementedException();
        }

        public virtual void LogInformationSummary(string data)
        {
            _logger.LogInformation(data);
        }

        public virtual void LogMinimal(string data)
        {
            _logger.LogInformation(data);
        }

        public virtual void LogVerbose(string data)
        {
            _logger.LogInformation(data);
        }

        public virtual void LogWarning(string data)
        {
            _logger.LogWarning(data);
        }
    }
}
