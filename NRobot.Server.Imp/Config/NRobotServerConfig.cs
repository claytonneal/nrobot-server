using System;
using System.Collections.Generic;
using System.Configuration;
using NRobot.Server.Imp.XmlConfig;

namespace NRobot.Server.Imp.Config
{
	/// <summary>
	/// Main configuration for NRobot service
	/// </summary>
	public class NRobotServerConfig
	{
		

        public int Port { get; set; }
        public Dictionary<String, LibraryConfig> AssemblyConfigs = new Dictionary<String, LibraryConfig>();


        private void AddDomainConfig(LibraryConfig config)
        {
            if (config == null) throw new ConfigurationErrorsException("Config specified is null");
            if (String.IsNullOrEmpty(config.TypeName)) throw new ConfigurationErrorsException("Config has not Type defined");
            AssemblyConfigs.Add(config.TypeName, config);
        }

        public static NRobotServerConfig LoadXmlConfiguration()
        {
            var xmlconfig = NRobotServerConfiguration.GetConfig();
            var result = new NRobotServerConfig();
            //get port
            result.Port = int.Parse(xmlconfig.Port.Number);
            //get keyword assemblies
            foreach (AssemblyElement xmlasm in xmlconfig.Assemblies)
            {
                result.AddDomainConfig(new LibraryConfig() { Assembly = xmlasm.Name, TypeName = xmlasm.Type, Documentation = xmlasm.DocFile });
            }
            return result;
        }








       
		
		
	}
	
	
}
