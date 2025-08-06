
namespace Krolti.DatabaseSO
{
    internal enum LogLevel
    {
        Log,
        Warn,
        Error,
        Throw
    }



    internal static class DebugDB
    {
        public static void Log<T>(string message, T item = null) where T : class, IDatabaseItem
        {
            Log<T>(message, LogLevel.Log, item);
        }

        public static void Warn<T>(string message, T item = null) where T : class, IDatabaseItem
        {
            Log<T>(message, LogLevel.Warn, item);
        }

        public static void Error<T>(string message, T item = null) where T : class, IDatabaseItem
        {
            Log<T>(message, LogLevel.Error, item);
        }

        public static void Throw<T>(string message, T item = null) where T : class, IDatabaseItem
        {
            Log<T>(message, LogLevel.Throw, item);
        }


        private static void Log<T>(string message, LogLevel logLevel, T item = null) where T : class, IDatabaseItem
        {
            string endMessage = item == null ? Format<T>(message) : FormatID<T>(message, item);

            LogBased(endMessage, logLevel);
        }



        private static void LogBased(string message, LogLevel logLevel)
        {
            switch (logLevel)
            {
                case LogLevel.Log:
                    UnityEngine.Debug.Log(message); break;

                case LogLevel.Warn:
                    UnityEngine.Debug.LogWarning(message); break;

                case LogLevel.Error:
                    UnityEngine.Debug.LogError(message); break;

                case LogLevel.Throw:
                    throw new DatabaseException(message);
            }
        }



        private static string Format<T>(string message) where T : class, IDatabaseItem
        {
            string formatted = string.Format("[{0}<{2}>] {1} Type: {2}",
                nameof(Database<T>),
                message,
                typeof(T)
                );

            return formatted;
        }

        private static string FormatID<T>(string message, T item) where T : class, IDatabaseItem
        {
            return Format<T>($"{message}, ID: {item.ID}");
        }
    }



    internal class DatabaseException : System.Exception
    {
        public DatabaseException(string message) : base(message) { }
    }


}
