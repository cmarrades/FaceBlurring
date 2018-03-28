using System;
using log4net;

namespace CMarrades.FaceBlurring.Global.Logging
{
    public static class Logger
    {
        private static readonly ILog _logger = LogManager.GetLogger(typeof(Logger));
        public static void Info(string message)
        {
            Console.WriteLine(message);
            _logger.Info(message);
        }

        public static void Error(string message)
        {
            Console.WriteLine($"Error {message}");
            _logger.Error(message);

        }
        public static void Error(string message, Exception ex)
        {
            Console.WriteLine($"Error {ex.Message}");
            _logger.Error(message, ex);
        }
        public static void Error(Exception ex)
        {
            Console.WriteLine($"Error {ex.Message}");
            _logger.Error(ex);
        }

    }
}
