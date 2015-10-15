using System;
using System.Collections.Generic;
using SimpleConfigurationManager;

namespace Monitor
{
    /// <summary>
    /// Example class demonstrating usage of <see cref="SimpleConfigurationManager.SConfigManager"/>. <br/><br/>
    /// </summary>
    public class MonitorAgent
    {
        #region [FIELDS]
        /// <summary>
        /// Key in app.config file for setting with path to the XML configuration.
        /// </summary>
        private static string CONFIGURATION_SETTING_KEY = "configurationFilePath";

        /// <summary>
        /// Instance of <see cref="SimpleConfigurationManager.SConfigManager"/> containing application configuration.
        /// </summary>
        private SConfigManager _configuration;

        /// <summary>
        /// List of directory watchers.
        /// </summary>
        private List<DirectoryWatcher> _directoryWatchers = new List<DirectoryWatcher>();

        /// <summary>
        /// Instance of <see cref="Monitor.Logger"/> class to log application messages.
        /// </summary>
        private Logger logger;
        #endregion

        /// <summary>
        /// Monitor initializes application by reading configuration, parsing it and initializing child-components.
        /// </summary>
        public MonitorAgent()
        {

            // Get path to the configuration file using settings in app.config.
            // Standard configuration is used only to get the path to our custom configuration.
            string configurationFilePath = 
                System.Configuration.ConfigurationManager.AppSettings[CONFIGURATION_SETTING_KEY];

            // Throw exception if cannot get information about xml configuration file.
            if (String.IsNullOrEmpty(configurationFilePath)) { 
                throw new ArgumentException("Could not find setting with path to ther configuration file. "
                    +"Please check your config file contains key \""+ 
                    CONFIGURATION_SETTING_KEY+"\" and that key is set properly");
            }

            Console.WriteLine("Initializing configuration at root level...");
            // Use static method of SConfigManager class to read configuration from the file.
            string configurationXml = SConfigManager.ReadConfigFromFile(configurationFilePath);
            
            // Initialize SConfigManager using configuration string.
            _configuration = new SConfigManager(configurationXml);
            Console.WriteLine("Initialized." + System.Environment.NewLine);

            Console.WriteLine("Initializing logger...");
            // Initialize logger by passing child of root configuration into its constructor.
            logger = new Logger(_configuration.Children["Logger"]);
            
            logger.Log("Logger initialised.");
            Console.WriteLine("Logger initialized." + System.Environment.NewLine);

            // DirectoryWatchers
            if (_configuration.Children.ContainsKey("Watchers"))
            {
                // Iterate Children of "Watchers" child of root configuration
                foreach (KeyValuePair<string, SConfigManager> element in _configuration.Children["Watchers"].Children)
                {
                    SConfigManager config = (SConfigManager)element.Value;

                    // Initialize new directoryWatcher by passing configuration into its constructor.
                    Console.WriteLine("Initializing DirectoryWatcher: "  +config.Name + "...");
                    DirectoryWatcher directoryWatcher = new DirectoryWatcher(config);
                    directoryWatcher.LogHandler += new LogEventHandler(OnAction);
                    Console.WriteLine("DirectoryWatcher initialized: " + config.Name + "..." 
                        + System.Environment.NewLine);

                    // Add new directoryWatcher to the list of directoryWatchers
                    _directoryWatchers.Add(directoryWatcher);
                }
            }

            Console.WriteLine("Watchers are now listening for any changes in watched directories."+
                "Make any action, like create or rename a file in those directories to see its working."
                + System.Environment.NewLine);
        }

        /// <summary>
        /// Logs message from watchers using logger.
        /// </summary>
        // <param name="sender">Event's sender.</param>
        /// <param name="eventArgs">Object with information about event.</param>
        private void OnAction(Object sender, LogEventArgs eventArgs)
        {
            logger.Log(eventArgs.Message);
        }

    }
}
