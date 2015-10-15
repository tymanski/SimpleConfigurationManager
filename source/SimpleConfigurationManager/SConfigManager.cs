using System;
using System.Collections.Generic;
using System.Xml;

namespace SimpleConfigurationManager
{
    /// <summary>
    /// Configuration manager class. 
    /// </summary>
    public class SConfigManager
    {
        #region [Fields]
        /// <summary>
        /// Name of the configuration section.
        /// </summary>
        public string Name { get; internal set; }

        /// <summary>
        /// Basic settings in a form of key-value pairs.
        /// </summary>
        public Dictionary<string, string> Settings { get; set; }

        /// <summary>
        /// Custom XML. Can be used to store custom XML data, that cannot be presented as standard key-value pairs.
        /// </summary>
        public string CustomXml { get; set; }

        /// <summary>
        /// Children of a configuration section. Children as same type as parents. A Name of each children is a key in this collection.
        /// </summary>
        public Dictionary<string, SConfigManager> Children { get; set; }

        /// <summary>
        /// The config in XML format that was used to initialize component.
        /// </summary>
        private string _configXml;
        #endregion

        #region [Constructor]
        /// <summary>
        /// Initializes a new instance of the <see cref="TimConfigurationManager.ConfigurationManager"/> class. 
        /// It parses given configuration which have to be in proper XML format.
        /// </summary>
        /// <example>  
        /// This sample shows how to call the initialize the <see cref="TimConfigurationManager.ConfigurationManager"/> class.
        /// <code> 
        /// class TestClass  
        /// { 
        ///     static int Main()  
        ///     { 
        ///         // Prepare path to file with configuration
        ///         string pathToFile = "path/to/file.txt";
        /// 
        ///         // Call static method to read content of a configuration file
        ///         string configString = ConfigurationManager.ReadConfigFromFile(pathToFile);
        /// 
        ///         // Initialize new Configuration Manager class by passing string with configuration
        ///         ConfigurationManager configurationManager = new ConfigurationManager(configString);
        ///         
        ///         //...
        ///     } 
        /// } 
        /// </code> 
        /// </example> 
        /// <param name="configuration">String containing configuration (in XML format) to be parsed into object.</param>
        /// <exception cref="System.Exception"></exception>
        public SConfigManager(string configuration)
        {
            this._configXml = configuration;
            XmlDocument configXml = null;

            // Initializing XmlDocument
            try
            {
                configXml = new XmlDocument();
                configXml.LoadXml(configuration);
            }
            catch (Exception e)
            {
                Exception exception = new Exception("Cannot parse XML file", e);
                throw exception;
            }

            // Read Name of the configuration section
            try
            {
                Name = configXml.DocumentElement.Attributes["name"].Value;

                if (Name == "")
                {
                    throw new Exception();
                }
            }
            catch (Exception)
            {
                ArgumentException exception = new ArgumentException("Argument cannot null nor empty", "name");
                throw exception;
            }

            try
            {
                // parse settings
                Settings = new Dictionary<string, string>();

                XmlNodeList xnList = configXml.SelectNodes("/configuration/settings//setting");

                foreach (XmlNode xn in xnList)
                {
                    string key = xn.Attributes["key"].Value;
                    string value = xn.Attributes["value"].Value;
                    Settings.Add(key, value);
                }

                // get custom xml
                xnList = configXml.SelectNodes("/configuration/custom");

                if (xnList.Count > 0)
                {
                    CustomXml = xnList[0].InnerXml.ToString();
                }

                // get children
                Children = new Dictionary<string, SConfigManager>();

                xnList = configXml.SelectNodes("/configuration/children/configuration");

                if (xnList.Count > 0)
                {
                    foreach (XmlNode xmlNode in xnList)
                    {
                        SConfigManager childConfig = new SConfigManager(xmlNode.OuterXml.ToString());

                        Children.Add(childConfig.Name, childConfig);
                    }
                }
            }
            catch (Exception ex)
            {
                Exception exception = new Exception("Error while processing XML", ex);
                throw exception;
            }
        }
        #endregion

        #region [PublicMethods]
        /// <summary>
        /// Reads the configuration string from file and returns it.
        /// </summary>
        /// <remarks>This method is handling only reading operation and casting to XML. No XML validation is done here.</remarks>
        /// <example>  
        /// This sample shows how to use static <see cref="ReadConfigFromFile"/> method.
        /// <code> 
        /// class TestClass  
        /// { 
        ///     static int Main()  
        ///     { 
        ///         // Prepare path to file with configuration
        ///         string pathToFile = "path/to/file.txt";
        /// 
        ///         // Call static method to read content of a configuration file
        ///         string configString = ConfigurationManager.ReadConfigFromFile(pathToFile);
        ///     } 
        /// } 
        /// </code> 
        /// </example> 
        /// <param name="path">Path to the file with configuration.</param>
        /// <exception cref="System.Exception"></exception>
        /// <returns>Content of the file.</returns>
        public static string ReadConfigFromFile(string path)
        {
            XmlDocument content = new XmlDocument();

            try
            {
                content.Load(path);
            }
            catch (Exception ex)
            {
                Exception exception = new Exception("Cannot load XML from file.", ex);
                throw exception;
            }

            return content.InnerXml;
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="System.String" /> that represents this instance.</returns>
        public override string ToString()
        {
            return _configXml;
        }
        #endregion
    }
}
