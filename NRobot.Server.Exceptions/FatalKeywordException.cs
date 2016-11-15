using System;
using System.Runtime.Serialization;

namespace NRobot.Server.Exceptions
{
	/// <summary>
	/// Desctiption of FatalKeywordException.
	/// </summary>
	public class FatalKeywordException : Exception, ISerializable
	{
		public FatalKeywordException()
		{
		}

	 	public FatalKeywordException(string message) : base(message)
		{
		}

		public FatalKeywordException(string message, Exception innerException) : base(message, innerException)
		{
		}

		// This constructor is needed for serialization.
		protected FatalKeywordException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}
}