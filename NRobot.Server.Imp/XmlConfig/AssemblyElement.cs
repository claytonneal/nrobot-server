using System;
using System.Configuration;

namespace NRobot.Server.Imp.XmlConfig
{
	/// <summary>
	/// Assembly configuration element
	/// </summary>
	public class AssemblyElement : ConfigurationElement
	{
		
		[ConfigurationProperty("name",IsRequired=true)]
		public string Name
		{
			get
			{
				return this["name"] as string;
			}
		}
		
		[ConfigurationProperty("type",IsRequired=true)]
		public string Type
		{
			get
			{
				return this["type"] as string;
			}
		}
		
		[ConfigurationProperty("docfile",IsRequired = false)]
		public string DocFile
		{
			get
			{
				try
				{
					return this["docfile"] as string;
				}
				catch
				{
					return String.Empty;
				}
			}
		}
		
		
	}
}
