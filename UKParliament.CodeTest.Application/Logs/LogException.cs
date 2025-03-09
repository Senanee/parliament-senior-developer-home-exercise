using Serilog;

namespace UKParliament.CodeTest.Application.Logs
{
    public static class LogException
    {
        public static void LogExceptions(Exception exception)
        {
            LogToFile(exception.Message);
            LogToConsole(exception.Message);
            LogToDebugger(exception.Message);

        }

        public static void LogToDebugger(string message) => Log.Debug(message);

        public static void LogToConsole(string message) => Log.Warning(message);

        public static void LogToFile(string message) => Log.Information(message);
    }
}
