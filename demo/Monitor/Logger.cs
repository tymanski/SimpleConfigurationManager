using System;
using SimpleConfigurationManager;
using System.IO;

namespace Monitor
{
    /// <summary>
    /// Logic of logging activity to file.
    /// </summary>
    public class Logger
    {
        /// <summary>
        /// Logger configuration passed from parent.
        /// </summary>
        private SConfigManager _configuration;
        
        // Logger enabled status 
        private bool _enabled = false;

        /// <summary>
        /// Initializes Logger instance.
        /// </summary>
        /// <param name="configuration">Configuration for logger.</param>
        public Logger(SConfigManager configuration)
        {
            // Configuration passed from parent, who initialized this instance.
            _configuration = configuration;

            Console.WriteLine("Logs are saved to: " + _configuration.Settings["LogDirectory"]);

            _enabled = Convert.ToBoolean(_configuration.Settings["Enabled"]);
        }

        /// <summary>
        /// Performs log action. Writes log message from the message parameter to the file.
        /// </summary>
        /// <param name="message">Log message</param>
        public void Log(string message)
        {
            if (_enabled)
            {
                DateTime now = DateTime.Now;

                string logFileName = _configuration.Settings["LogFilePrefix"] + 
                    now.ToString("yyyyMMdd_hhmmss") + "_" + System.Guid.NewGuid().ToString() + ".log";

                File.WriteAllText(Path.Combine(_configuration.Settings["LogDirectory"],logFileName), message);
            }
        }

    }
}
