namespace HvoyaApplication.Utilities
{
    public static class Logger
    {
        private static readonly string LogFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Logs", "logs.csv");

        static Logger()
        {
            var logDirectory = Path.GetDirectoryName(LogFilePath);
            if (!Directory.Exists(logDirectory))
            {
                Directory.CreateDirectory(logDirectory);
            }

            if (!File.Exists(LogFilePath))
            {
                File.AppendAllText(LogFilePath, "DateTime, Controller, Action, User, Details\n");
            }
        }

        public static void Log(string controller, string action, string user, string details)
        {
            try
            {
                string logEntry = $"{DateTime.Now:yyyy-MM-dd:mm:ss}, {controller}, {action}, {user}, {details}\n";
                File.AppendAllText(LogFilePath, logEntry);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Logger Error: {ex.Message}");
            }
        }
    }
}
