## Description

**SimpleConfigurationManager** (**SConfigManager**) is a component to deal with configurations in .NET apllications. For more information please refer to the article on [codeproject](link)


## Code Example

Example usage of the class:

	string configurationXml = SConfigManager.ReadConfigFromFile("D:/myapp/configuration.xml");

	// Initialize SConfigManager using configuration string.
	SConfigManager configuration = new SConfigManager(configurationXml);

	// Get configuration name:
	Console.WriteLine(configuration.Name);

	// Get some settings from configuration
	string agentName = configuration.Settings["AgentName"];

	// Get subconfiguration
	SConfigManager loggerConfiguration = configuration.Children["Logger"];
	Logger logger = new Logger(loggerConfiguration);

## Installation

### Using library
To use SConfigManager just add reference to the .dll file inside /build/SimpleConfigurationManager_vx.x/

To see wortking example please refer to the demo project attached.

### Running demo project
To run demo project make sure you have directories specified in configuration file.

## Contributors
Would be grateful for any feedback: timmy.coder@gmail.com 
