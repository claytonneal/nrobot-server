using System.Configuration;

namespace NRobot.Server.Imp.XmlConfig
{
	/// <summary>
	/// Configuration section handler
	/// </summary>
	public class NRobotServerConfiguration : ConfigurationSection
	{
		
		private const string CConfigSection = "NRobotServerConfiguration";
		
		public static NRobotServerConfiguration GetConfig()
		{
			return (NRobotServerConfiguration)ConfigurationManager.GetSection(CConfigSection) ?? new NRobotServerConfiguration();
		}
		
		[ConfigurationProperty("assemblies")]
		public AssemblyElementCollection Assemblies
		{
			get
			{
				return (AssemblyElementCollection)this["assemblies"] ?? new AssemblyElementCollection();
			}
		}
		
		[ConfigurationProperty("port")]
		public PortElement Port
		{
			get
			{
				return (PortElement)this["port"] ?? new PortElement();
			}
		}
		
	}
}
