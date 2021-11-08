using System.Reflection;

namespace BulletinTable.Utils
{
    public sealed class LOG
    {
        private static readonly LOG inst = new LOG();
        private const string FILE_EXT = ".log";
        private readonly string datetimeFormat;
        private readonly string logFilename;

        static LOG()
        {
        }

        /// <summary>
        /// Initiate an instance of SimpleLogger class constructor.
        /// If log file does not exist, it will be created automatically.
        /// </summary>
        private LOG()
        {
            datetimeFormat = "yyyy-MM-dd HH:mm:ss.fff";
            logFilename = Assembly.GetExecutingAssembly().GetName().Name + FILE_EXT;

            // Log file header line
            var logHeader = logFilename + " is created.";
            if (!File.Exists(logFilename))
            {
                WriteLine(DateTime.Now.ToString(datetimeFormat) + " " + logHeader, false);
            }
        }

        public static LOG Inst => inst;

        /// <summary>
        /// Log a DEBUG message
        /// </summary>
        /// <param name="text">Message</param>
        public void Debug(string text)
        {
            WriteFormattedLog(LogLevel.DEBUG, text);
        }

        /// <summary>
        /// Log an ERROR message
        /// </summary>
        /// <param name="text">Message</param>
        public void Error(string text)
        {
            WriteFormattedLog(LogLevel.ERROR, text);
        }

        /// <summary>
        /// Log an ERROR message with Method and class name
        /// </summary>
        /// <param name="text">Message</param>
        /// <param name="methodBase">MethodBase</param>
        public void Error(string text, MethodBase methodBase)
        {
            WriteFormattedLog(LogLevel.ERROR, $@"{text} @{methodBase.DeclaringType?.Name}.{methodBase.Name}");
        }

        /// <summary>
        /// Log a FATAL ERROR message
        /// </summary>
        /// <param name="text">Message</param>
        public void Fatal(string text)
        {
            WriteFormattedLog(LogLevel.FATAL, text);
        }

        /// <summary>
        /// Log an FATAL message with Method and class name
        /// </summary>
        /// <param name="text">Message</param>
        /// <param name="methodBase">MethodBase</param>
        public void Fatal(string text, MethodBase methodBase)
        {
            WriteFormattedLog(LogLevel.FATAL, $@"{text} @{methodBase.DeclaringType?.Name}.{methodBase.Name}");
        }

        /// <summary>
        /// Log an INFO message
        /// </summary>
        /// <param name="text">Message</param>
        public void Info(string text)
        {
            WriteFormattedLog(LogLevel.INFO, text);
        }

        /// <summary>
        /// Log a TRACE message
        /// </summary>
        /// <param name="text">Message</param>
        public void Trace(string text)
        {
            WriteFormattedLog(LogLevel.TRACE, text);
        }

        /// <summary>
        /// Log a WARNING message
        /// </summary>
        /// <param name="text">Message</param>
        public void Warning(string text)
        {
            WriteFormattedLog(LogLevel.WARNING, text);
        }

        private void WriteLine(string text, bool append = true)
        {
            using var writer =
                new StreamWriter(logFilename, append, System.Text.Encoding.UTF8);
            if (string.IsNullOrEmpty(text))
            {
                return;
            }

            writer.WriteLine(text);
            System.Diagnostics.Debug.Print(text);
        }

        private void WriteFormattedLog(LogLevel level, string text)
        {
            string pretext;
            switch (level)
            {
                case LogLevel.TRACE:
                    pretext = DateTime.Now.ToString(datetimeFormat) + " [TRACE]   ";
                    break;
                case LogLevel.INFO:
                    pretext = DateTime.Now.ToString(datetimeFormat) + " [INFO]    ";
                    break;
                case LogLevel.DEBUG:
                    pretext = DateTime.Now.ToString(datetimeFormat) + " [DEBUG]   ";
                    break;
                case LogLevel.WARNING:
                    pretext = DateTime.Now.ToString(datetimeFormat) + " [WARNING] ";
                    break;
                case LogLevel.ERROR:
                    pretext = DateTime.Now.ToString(datetimeFormat) + " [ERROR]   ";
                    break;
                case LogLevel.FATAL:
                    pretext = DateTime.Now.ToString(datetimeFormat) + " [FATAL]   ";
                    break;
                default:
                    pretext = "";
                    break;
            }

            WriteLine(pretext + text);
        }

        [Flags]
        private enum LogLevel
        {
            TRACE,
            INFO,
            DEBUG,
            WARNING,
            ERROR,
            FATAL
        }
    }
}
