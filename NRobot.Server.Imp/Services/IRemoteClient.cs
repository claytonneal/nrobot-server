using CookComputing.XmlRpc;

namespace NRobot.Server.Imp.Services
{
	/// <summary>
	/// Interface to define a client proxy for robot remote 
	/// </summary>
	public interface IRemoteClient : IRemoteService, IXmlRpcProxy
	{
		
	}
}
