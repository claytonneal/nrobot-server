using System.Configuration;

namespace NRobot.Server.Imp.XmlConfig
{

	public class AssemblyElementCollection : ConfigurationElementCollection
	{
		
		public AssemblyElement this[int index]
		{
			get
			{
				return BaseGet(index) as AssemblyElement;
			}
			set
			{
				if (BaseGet(index) != null)
				{
					BaseRemoveAt(index);
				}
				BaseAdd(index,value);
			}
		}
		
		protected override ConfigurationElement CreateNewElement()
		{
			return new AssemblyElement();
		}
		
		
		protected override object GetElementKey(ConfigurationElement element)
		{
			return ((AssemblyElement)element).Type;
		}
		
	}
}
