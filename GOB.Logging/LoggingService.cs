using System.Diagnostics;

namespace GOB.Logging
{
    public class LoggingService
    {
        public static class Categories
        {
            public static string TimerJob = "Timer Job";
            public static string Nav = "NAV";
            public static string Csom = "SharePoint CSOM";
            public static string AzureAd = "Azure AD";
            public static string Office365 = "Office365";
            public static string Custom = "Custom";
        }

        public static string LogName => "GOB2";

        public LoggingService()
        {
        }

        public static void Log(string source, EventLogEntryType logEntryType, string message, string categoryName = "")
        {
            if (string.IsNullOrEmpty(message))
                return;

            if (!EventLog.SourceExists(source))
                EventLog.CreateEventSource(source, LogName);

            EventLog.WriteEntry(source, message, logEntryType);
        }

        private static void Log(string sourceMessage, EventLogEntryType logEntryType, bool logStackTrace = false, string categoryName = "")
        {
            var stackTrace = new StackTrace();
            var sf = stackTrace.GetFrame(2);
            var sfMethod = sf.GetMethod();
            var functionName = sf.GetMethod().Name;

            var source = !string.IsNullOrEmpty(sfMethod.DeclaringType?.FullName) ? sfMethod.DeclaringType.FullName : functionName;

            var message = $"Name of Function: \"{functionName}\".";

            if (!string.IsNullOrEmpty(categoryName))
                message += $"\r\nCategory: \"{categoryName}\"";

            message += $"\r\nMessage: {sourceMessage}";

            message += $"\r\n{stackTrace}";

            Log(source, logEntryType, message, categoryName);
        }

        public static void LogError(string errorMessage, bool logStackTrace = true, string categoryName = "")
        {
            Log(errorMessage, EventLogEntryType.Error, logStackTrace, categoryName);
        }

        public static void LogInformation(string sourceMessage, bool logStackTrace = false, string categoryName = "")
        {
            Log(sourceMessage, EventLogEntryType.Information, logStackTrace, categoryName);
        }

        public static void LogWarning(string sourceMessage, bool logStackTrace = false, string categoryName = "")
        {
            Log(sourceMessage, EventLogEntryType.Warning, logStackTrace, categoryName);
        }

        public static void Log(string sourceMessage, EventLogEntryType logEntryType, IGobLoggable logObject, string categoryName = "")
        {
            Log(sourceMessage, logEntryType, logObject.GetLogMessage(), categoryName);
        }
    }
}
