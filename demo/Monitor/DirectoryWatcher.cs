using System;
using System.IO;
using SimpleConfigurationManager;

namespace Monitor
{
    /// <summary>
    /// Class used to watch changes in directories. It monitors for create, rename and delete actions in directory and 
    /// throws exception, if and of those actions occurs.
    /// </summary>
    public class DirectoryWatcher
    {
        /// <summary>
        /// Log event handler.
        /// </summary>
        public event LogEventHandler LogHandler;

        /// <summary>
        /// Configuration of directorywatcher passed from parent.
        /// </summary>
        private SConfigManager _configuration;

        /// <summary>
        /// FileSystemWatcher that watches directories for changes.
        /// </summary>
        private FileSystemWatcher _watcher;


        /// <summary>
        /// Initializes the DirectoryWatcher instance.
        /// </summary>
        /// <param name="configuration">Configuration object conatining all necessary configuration for 
        /// this class.</param>
        public DirectoryWatcher(SConfigManager configuration)
        {
            // Configuration passed from parent, who initialized this instance.
            _configuration = configuration;

            Console.WriteLine("Watched directory: : " + _configuration.Settings["Directory"]);

            _watcher = new FileSystemWatcher();
            _watcher.Path = _configuration.Settings["Directory"];
            _watcher.NotifyFilter = NotifyFilters.FileName | NotifyFilters.LastWrite;
            _watcher.Filter = _configuration.Settings["Filter"];
            _watcher.Created += new FileSystemEventHandler(OnCreated);
            _watcher.Renamed += new RenamedEventHandler(OnRenamed);
            _watcher.Deleted += new FileSystemEventHandler(OnDeleted);
            _watcher.EnableRaisingEvents = true;
        }

        /// <summary>
        /// Action called when file has been created.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        private void OnCreated(object source, FileSystemEventArgs e)
        {
            string message = "Watcher: " + _configuration.Name + " noticed change action." 
                + System.Environment.NewLine +
                "File: " + e.FullPath + System.Environment.NewLine +
                "ActionType: " + e.ChangeType.ToString() + System.Environment.NewLine;

            Console.Write(message + System.Environment.NewLine);

            LogHandler(this, new LogEventArgs(message));
        }

        /// <summary>
        /// Action called when file has been renamed.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        private void OnRenamed(object source, RenamedEventArgs e)
        {
            string message = "Watcher: " + _configuration.Settings["Name"] + " noticed rename action." 
                + System.Environment.NewLine +
            "Old path: " + e.OldFullPath + System.Environment.NewLine +
            "New name: " + e.FullPath + System.Environment.NewLine;

            Console.Write(message + System.Environment.NewLine);

            LogHandler(this, new LogEventArgs(message));
        }

        /// <summary>
        /// Action called when file has been deleted.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        private void OnDeleted(object source, FileSystemEventArgs e)
        {
            string message = "Watcher: " + _configuration.Settings["Name"] + " noticed delete action." 
                + System.Environment.NewLine +
            "File: " + e.FullPath + System.Environment.NewLine +
            "ActionType: " + e.ChangeType.ToString() + System.Environment.NewLine;

            Console.Write(message + System.Environment.NewLine);

            LogHandler(this, new LogEventArgs(message));
        }
    }
}
