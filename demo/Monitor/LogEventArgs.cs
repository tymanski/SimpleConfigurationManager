using System;

namespace Monitor
{
    /// <summary>
    /// Log event data.
    /// </summary>
    public class LogEventArgs : EventArgs
    {
        /// <summary>
        /// Message to be logged.
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Initializes class.
        /// </summary>
        /// <param name="message"></param>
        public LogEventArgs(string message)
        {
            Message = message;
        }
    }
}
