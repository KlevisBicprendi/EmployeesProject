using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL_Punonjes.Services.Singletone
{
    public interface ILoggerService
    {
        void LogError(Exception exception);
        void LogError(string message);
        void Log(string logType, string logData);
    }
    public class LoggerService : ILoggerService
    {
        private readonly object _lock = new object();
        private string _logDirectory;
        public LoggerService(IConfiguration configuration)
        {
            _logDirectory = configuration["LogsDirectory"];
            if (!System.IO.Directory.Exists(_logDirectory))
            {
                System.IO.Directory.CreateDirectory(_logDirectory);
            }
        }
        public void LogError(Exception exception)
        {
            var logdata = $"Error : {exception.Message} {exception.StackTrace}";
        }

        public void LogError(string message)
        {
            lock (_lock)
            {
                var logData = $"Error : {message}";
                Log("Error",logData);
            }
        }
        public void Log(string logType, string logData)
        {
            lock (_lock)
            {
                var logFileName = $"{_logDirectory}/{logType}-{DateTime.Now:yyyy-MM-dd}.log";
                var logMessage = $"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")} {Environment.NewLine}{logData}{Environment.NewLine}";
                System.IO.File.AppendAllText(logFileName, logMessage);
            }
            }
        }
}