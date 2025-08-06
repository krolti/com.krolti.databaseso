
namespace Krolti.DatabaseSO
{
    internal enum LogLevel
    {
        Log,
        Warn,
        Error
    }


    internal static class DebugDB
    {
        internal static void Log<T>(string message, T item = null) where T : class, IDatabaseItem
        {
            CreateLog<T>(message, LogLevel.Log, item);
        }

        internal static void LogFormat<T>(string message, params object[] args) where T : class, IDatabaseItem
        {
            CreateLog<T>(string.Format(message, args), LogLevel.Log);
        }


        internal static void Warn<T>(string message, T item = null) where T : class, IDatabaseItem
        {
            CreateLog<T>(message, LogLevel.Warn, item);
        }

        internal static void WarnFormat<T>(string message, params object[] args) where T : class, IDatabaseItem
        {
            CreateLog<T>(string.Format(message, args), LogLevel.Warn);
        }


        internal static void Error<T>(string message, T item = null) where T : class, IDatabaseItem
        {
            CreateLog<T>(message, LogLevel.Error, item);
        }

        internal static void ErrorFormat<T>(string message, params object[] args) where T : class, IDatabaseItem
        {
            CreateLog<T>(string.Format(message, args), LogLevel.Error);
        }


        internal static void Throw<T>(string message, T item = null) where T : class, IDatabaseItem
        {
            string endMessage = item == null ? message.Format<T>() : message.Format(item);

            throw new DatabaseException(endMessage);
        }


        internal static System.Exception Exception<T>(string message, T item = null) where T : class, IDatabaseItem
        {
            string endMessage = item == null ? message.Format<T>() : message.Format(item);

            return new DatabaseException(endMessage);
        }


        internal static System.Exception ExceptionFormat<T>(string message, params object[] args) where T : class, IDatabaseItem
        {
            string endMessage = string.Format(message, args).Format<T>();

            return new DatabaseException(endMessage);
        }


        private static void CreateLog<T>(string message, LogLevel logLevel, T item = null) where T : class, IDatabaseItem
        {
            string endMessage = item == null ? message.Format<T>() : message.Format(item);

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
            }
        }

    }


    internal class DatabaseException : System.Exception
    {
        public DatabaseException(string message) : base(message) { }
    }
}