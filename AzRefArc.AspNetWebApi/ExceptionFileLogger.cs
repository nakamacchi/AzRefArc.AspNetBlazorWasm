using System.Reflection;
using System.Text;
using System.Collections.Concurrent;

namespace AzRefArc.AspNetWebApi
{
    public sealed class ExceptionFileLogger : ILogger
    {
        public IDisposable? BeginScope<TState>(TState state) where TState : notnull => default!;

        public bool IsEnabled(LogLevel logLevel)
        {
            return (logLevel == LogLevel.Warning || logLevel == LogLevel.Error);
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
        {
            if (!IsEnabled(logLevel)) return;

            string fileName = string.Format("{0:yyyyMMdd}.txt", DateTime.Now);
            string folder = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData, Environment.SpecialFolderOption.None) + @"\" + Assembly.GetEntryAssembly()!.GetName().Name;
            Directory.CreateDirectory(folder);
            string filePath = Path.Combine(folder, fileName);
            File.AppendAllText(filePath, $"{formatter(state, exception)}");
        }
    }

    public class ExceptionFileLoggerProvider : ILoggerProvider
    {
        private readonly ConcurrentDictionary<string, ExceptionFileLogger> _loggers = new ConcurrentDictionary<string, ExceptionFileLogger>();

        public ExceptionFileLoggerProvider()
        {
            // initialization code
        }

        public ILogger CreateLogger(string categoryName)
        {
            var logger = _loggers.GetOrAdd(categoryName, new ExceptionFileLogger());
            return logger;
        }

        public void Dispose()
        {
            _loggers.Clear();
        }
    }
}