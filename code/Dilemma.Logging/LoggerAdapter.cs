using System;
using NLog;

namespace Dilemma.Logging
{
    /// <summary>
    /// Implementation of ILogger, deriving from NLog.Logger
    /// </summary>
    public class LoggerAdapter : ILogger
    {
        /// <summary>
        /// Occurs when logger configuration changes.
        /// </summary>
        public event EventHandler<EventArgs> LoggerReconfigured
        {
            add { GetLogger().LoggerReconfigured += value; }
            remove { GetLogger().LoggerReconfigured -= value; }
        }


        public string Name { get { return GetLogger().Name; } }

        public LogFactory Factory { get { return GetLogger().Factory; } }

        public bool IsTraceEnabled { get { return GetLogger().IsTraceEnabled; } }

        public bool IsDebugEnabled { get { return GetLogger().IsDebugEnabled; } }

        public bool IsInfoEnabled { get { return GetLogger().IsInfoEnabled; } }

        public bool IsWarnEnabled { get { return GetLogger().IsWarnEnabled; } }

        public bool IsErrorEnabled { get { return GetLogger().IsErrorEnabled; } }

        public bool IsFatalEnabled { get { return GetLogger().IsErrorEnabled; } }

        public bool IsEnabled(LogLevel level)
        {
            return GetLogger().IsEnabled(level);
        }

        public void Log(LogEventInfo logEvent)
        {
            GetLogger().Log(logEvent);
        }

        public void Log(Type wrapperType, LogEventInfo logEvent)
        {
            GetLogger().Log(wrapperType, logEvent);
        }

        public void Log<T>(LogLevel level, T value)
        {
            GetLogger().Log(level, value);
        }

        public void Log<T>(LogLevel level, IFormatProvider formatProvider, T value)
        {
            GetLogger().Log(level, formatProvider, value);
        }

        public void Log(LogLevel level, LogMessageGenerator messageFunc)
        {
            GetLogger().Log(level, messageFunc);
        }

        [Obsolete("Use Log(LogLevel, String, Exception) method instead.")]
        public void LogException(LogLevel level, string message, Exception exception)
        {
            GetLogger().LogException(level, message, exception);
        }

        public void Log(LogLevel level, IFormatProvider formatProvider, string message, params object[] args)
        {
            GetLogger().Log(level, formatProvider, message, args);
        }

        public void Log(LogLevel level, string message)
        {
            GetLogger().Log(level, message);
        }

        public void Log(LogLevel level, string message, params object[] args)
        {
            GetLogger().Log(level, message, args);
        }

        public void Log<TArgument>(LogLevel level, IFormatProvider formatProvider, string message, TArgument argument)
        {
            GetLogger().Log(level, formatProvider, message, argument);
        }

        public void Log<TArgument>(LogLevel level, string message, TArgument argument)
        {
            GetLogger().Log(level, message, argument);
        }

        public void Log<TArgument1, TArgument2>(LogLevel level, IFormatProvider formatProvider, string message, TArgument1 argument1, TArgument2 argument2)
        {
            GetLogger().Log(level, formatProvider, message, argument1, argument2);
        }

        public void Log<TArgument1, TArgument2>(LogLevel level, string message, TArgument1 argument1, TArgument2 argument2)
        {
            GetLogger().Log(level, message, argument1, argument2);
        }

        public void Log<TArgument1, TArgument2, TArgument3>(LogLevel level, IFormatProvider formatProvider, string message, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3)
        {
            GetLogger().Log(level, formatProvider, message, argument1, argument2, argument3);
        }

        public void Log<TArgument1, TArgument2, TArgument3>(LogLevel level, string message, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3)
        {
            GetLogger().Log(level, message, argument1, argument2, argument3);
        }

        public void TraceException(string message, Exception exception)
        {
            GetLogger().TraceException(message, exception);
        }

        public void Trace<T>(T value)
        {
            GetLogger().Trace(value);
        }

        public void Trace<T>(IFormatProvider formatProvider, T value)
        {
            GetLogger().Trace(formatProvider, value);
        }

        public void Trace(LogMessageGenerator messageFunc)
        {
            GetLogger().Trace(messageFunc);
        }

        public void Trace(string message, Exception exception)
        {
            GetLogger().Trace(message, exception);
        }

        public void Trace(IFormatProvider formatProvider, string message, params object[] args)
        {
            GetLogger().Trace(formatProvider, message, args);
        }

        public void Trace(string message)
        {
            GetLogger().Trace(message);
        }

        public void Trace(string message, params object[] args)
        {
            GetLogger().Trace(message, args);
        }

        public void Trace<TArgument>(IFormatProvider formatProvider, string message, TArgument argument)
        {
            GetLogger().Trace(formatProvider, message, argument);
        }

        public void Trace<TArgument>(string message, TArgument argument)
        {
            GetLogger().Trace(message, argument);
        }

        public void Trace<TArgument1, TArgument2>(IFormatProvider formatProvider, string message, TArgument1 argument1, TArgument2 argument2)
        {
            GetLogger().Trace(formatProvider, message, argument1, argument2);
        }

        public void Trace<TArgument1, TArgument2>(string message, TArgument1 argument1, TArgument2 argument2)
        {
            GetLogger().Trace(message, argument1, argument2);
        }

        public void Trace<TArgument1, TArgument2, TArgument3>(IFormatProvider formatProvider, string message, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3)
        {
            GetLogger().Trace(formatProvider, message, argument1, argument2, argument3);
        }

        public void Trace<TArgument1, TArgument2, TArgument3>(string message, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3)
        {
            GetLogger().Trace(message, argument1, argument2, argument3);
        }

        public void DebugException(string message, Exception exception)
        {
            GetLogger().DebugException(message, exception);
        }

        public void Debug<T>(T value)
        {
            GetLogger().Debug(value);
        }

        public void Debug<T>(IFormatProvider formatProvider, T value)
        {
            GetLogger().Debug(formatProvider, value);
        }

        public void Debug(LogMessageGenerator messageFunc)
        {
            GetLogger().Debug(messageFunc);
        }

        public void Debug(string message, Exception exception)
        {
            GetLogger().Debug(message, exception);
        }

        public void Debug(IFormatProvider formatProvider, string message, params object[] args)
        {
            GetLogger().Debug(formatProvider, message, args);
        }

        public void Debug(string message)
        {
            GetLogger().Debug(message);
        }

        public void Debug(string message, params object[] args)
        {
            GetLogger().Debug(message, args);
        }

        public void Debug<TArgument>(IFormatProvider formatProvider, string message, TArgument argument)
        {
            GetLogger().Debug(formatProvider, message, argument);
        }

        public void Debug<TArgument>(string message, TArgument argument)
        {
            GetLogger().Debug(message, argument);
        }

        public void Debug<TArgument1, TArgument2>(IFormatProvider formatProvider, string message, TArgument1 argument1, TArgument2 argument2)
        {
            GetLogger().Debug(formatProvider, message, argument1, argument2);
        }

        public void Debug<TArgument1, TArgument2>(string message, TArgument1 argument1, TArgument2 argument2)
        {
            GetLogger().Debug(message, argument1, argument2);
        }

        public void Debug<TArgument1, TArgument2, TArgument3>(IFormatProvider formatProvider, string message, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3)
        {
            GetLogger().Debug(formatProvider, message, argument1, argument2, argument3);
        }

        public void Debug<TArgument1, TArgument2, TArgument3>(string message, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3)
        {
            GetLogger().Debug(message, argument1, argument2, argument3);
        }

        public void InfoException(string message, Exception exception)
        {
            GetLogger().InfoException(message, exception);
        }

        public void Info<T>(T value)
        {
            GetLogger().Info(value);
        }

        public void Info<T>(IFormatProvider formatProvider, T value)
        {
            GetLogger().Info(formatProvider, value);
        }

        public void Info(LogMessageGenerator messageFunc)
        {
            GetLogger().Info(messageFunc);
        }

        public void Info(string message, Exception exception)
        {
            GetLogger().Info(message, exception);
        }

        public void Info(IFormatProvider formatProvider, string message, params object[] args)
        {
            GetLogger().Info(formatProvider, message, args);
        }

        public void Info(string message)
        {
            GetLogger().Info(message);
        }

        public void Info(string message, params object[] args)
        {
            GetLogger().Info(message, args);
        }

        public void Info<TArgument>(IFormatProvider formatProvider, string message, TArgument argument)
        {
            GetLogger().Info(formatProvider, message, argument);
        }

        public void Info<TArgument>(string message, TArgument argument)
        {
            GetLogger().Info(message, argument);
        }

        public void Info<TArgument1, TArgument2>(IFormatProvider formatProvider, string message, TArgument1 argument1, TArgument2 argument2)
        {
            GetLogger().Info(formatProvider, message, argument1, argument2);
        }

        public void Info<TArgument1, TArgument2>(string message, TArgument1 argument1, TArgument2 argument2)
        {
            GetLogger().Info(message, argument1, argument2);
        }

        public void Info<TArgument1, TArgument2, TArgument3>(IFormatProvider formatProvider, string message, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3)
        {
            GetLogger().Info(formatProvider, message, argument1, argument2, argument3);
        }

        public void Info<TArgument1, TArgument2, TArgument3>(string message, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3)
        {
            GetLogger().Info(message, argument1, argument2, argument3);
        }

        public void WarnException(string message, Exception exception)
        {
            GetLogger().WarnException(message, exception);
        }

        public void Warn<T>(T value)
        {
            GetLogger().Warn(value);
        }

        public void Warn<T>(IFormatProvider formatProvider, T value)
        {
            GetLogger().Warn(formatProvider, value);
        }

        public void Warn(LogMessageGenerator messageFunc)
        {
            GetLogger().Warn(messageFunc);
        }

        public void Warn(string message, Exception exception)
        {
            GetLogger().Warn(message, exception);
        }

        public void Warn(IFormatProvider formatProvider, string message, params object[] args)
        {
            GetLogger().Warn(formatProvider, message, args);
        }

        public void Warn(string message)
        {
            GetLogger().Warn(message);
        }

        public void Warn(string message, params object[] args)
        {
            GetLogger().Warn(message, args);
        }

        public void Warn<TArgument>(IFormatProvider formatProvider, string message, TArgument argument)
        {
            GetLogger().Warn(formatProvider, message, argument);
        }

        public void Warn<TArgument>(string message, TArgument argument)
        {
            GetLogger().Warn(message, argument);
        }

        public void Warn<TArgument1, TArgument2>(IFormatProvider formatProvider, string message, TArgument1 argument1, TArgument2 argument2)
        {
            GetLogger().Warn(formatProvider, message, argument1, argument2);
        }

        public void Warn<TArgument1, TArgument2>(string message, TArgument1 argument1, TArgument2 argument2)
        {
            GetLogger().Warn(message, argument1, argument2);
        }

        public void Warn<TArgument1, TArgument2, TArgument3>(IFormatProvider formatProvider, string message, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3)
        {
            GetLogger().Warn(formatProvider, message, argument1, argument2, argument3);
        }

        public void Warn<TArgument1, TArgument2, TArgument3>(string message, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3)
        {
            GetLogger().Warn(message, argument1, argument2, argument3);
        }

        public void ErrorException(string message, Exception exception)
        {
            GetLogger().ErrorException(message, exception);
        }

        public void Error<T>(T value)
        {
            GetLogger().Error(value);
        }

        public void Error<T>(IFormatProvider formatProvider, T value)
        {
            GetLogger().Error(formatProvider, value);
        }

        public void Error(LogMessageGenerator messageFunc)
        {
            GetLogger().Error(messageFunc);
        }

        public void Error(string message, Exception exception)
        {
            GetLogger().Error(message, exception);
        }

        public void Error(IFormatProvider formatProvider, string message, params object[] args)
        {
            GetLogger().Error(formatProvider, message, args);
        }

        public void Error(string message)
        {
            GetLogger().Error(message);
        }

        public void Error(string message, params object[] args)
        {
            GetLogger().Error(message, args);
        }

        public void Error<TArgument>(IFormatProvider formatProvider, string message, TArgument argument)
        {
            GetLogger().Error(formatProvider, message, argument);
        }

        public void Error<TArgument>(string message, TArgument argument)
        {
            GetLogger().Error(message, argument);
        }

        public void Error<TArgument1, TArgument2>(IFormatProvider formatProvider, string message, TArgument1 argument1, TArgument2 argument2)
        {
            GetLogger().Error(formatProvider, message, argument1, argument2);
        }

        public void Error<TArgument1, TArgument2>(string message, TArgument1 argument1, TArgument2 argument2)
        {
            GetLogger().Error(message, argument1, argument2);
        }

        public void Error<TArgument1, TArgument2, TArgument3>(IFormatProvider formatProvider, string message, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3)
        {
            GetLogger().Error(formatProvider, message, argument1, argument2, argument3);
        }

        public void Error<TArgument1, TArgument2, TArgument3>(string message, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3)
        {
            GetLogger().Error(message, argument1, argument2, argument3);
        }

        public void FatalException(string message, Exception exception)
        {
            GetLogger().FatalException(message, exception);
        }

        public void Fatal<T>(T value)
        {
            GetLogger().Fatal(value);
        }

        public void Fatal<T>(IFormatProvider formatProvider, T value)
        {
            GetLogger().Fatal(formatProvider, value);
        }

        public void Fatal(LogMessageGenerator messageFunc)
        {
            GetLogger().Fatal(messageFunc);
        }

        public void Fatal(string message, Exception exception)
        {
            GetLogger().Fatal(message, exception);
        }

        public void Fatal(IFormatProvider formatProvider, string message, params object[] args)
        {
            GetLogger().Fatal(formatProvider, message, args);
        }

        public void Fatal(string message)
        {
            GetLogger().Fatal(message);
        }

        public void Fatal(string message, params object[] args)
        {
            GetLogger().Fatal(message, args);
        }

        public void Fatal<TArgument>(IFormatProvider formatProvider, string message, TArgument argument)
        {
            GetLogger().Fatal(formatProvider, message, argument);
        }

        public void Fatal<TArgument>(string message, TArgument argument)
        {
            GetLogger().Fatal(message, argument);
        }

        public void Fatal<TArgument1, TArgument2>(IFormatProvider formatProvider, string message, TArgument1 argument1, TArgument2 argument2)
        {
            GetLogger().Fatal(formatProvider, message, argument1, argument2);
        }

        public void Fatal<TArgument1, TArgument2>(string message, TArgument1 argument1, TArgument2 argument2)
        {
            GetLogger().Fatal(message, argument1, argument2);
        }

        public void Fatal<TArgument1, TArgument2, TArgument3>(IFormatProvider formatProvider, string message, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3)
        {
            GetLogger().Fatal(formatProvider, message, argument1, argument2, argument3);
        }

        public void Fatal<TArgument1, TArgument2, TArgument3>(string message, TArgument1 argument1, TArgument2 argument2, TArgument3 argument3)
        {
            GetLogger().Fatal(message, argument1, argument2, argument3);
        }

        private static Logger GetLogger()
        {
            return LogManager.GetLogger(Configuration.ServerGuid);
        }
    }
}