using System;
using System.IO;
using System.Threading.Tasks;

namespace TicketManagementSystem
{
    public static class Logger
    {
        private static readonly string logDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Logs");
        private static readonly string logFilePath = Path.Combine(logDirectory, "error.log");

        public static async Task LogErrorAsync(string message)
        {
            try
            {
                if (!Directory.Exists(logDirectory))
                    Directory.CreateDirectory(logDirectory);

                string logEntry = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] ERROR: {message}{Environment.NewLine}";
                await File.AppendAllTextAsync(logFilePath, logEntry);
            }
            catch
            {
                // Suppress any errors thrown while logging to avoid crashing the application
            }
        }

        public static async Task LogExceptionAsync(Exception ex)
        {
            string message = $"Exception: {ex.Message}\nStackTrace: {ex.StackTrace}";
            await LogErrorAsync(message);
        }
    }
}