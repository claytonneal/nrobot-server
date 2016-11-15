using System;
using System.Runtime.Serialization;

namespace NRobot.Server.Exceptions
{
	/// <summary>
	/// Desctiption of ContinuableKeywordException.
	/// </summary>
	public class ContinuableKeywordException : Exception, ISerializable
	{
		public ContinuableKeywordException()
		{
		}

	 	public ContinuableKeywordException(string message) : base(message)
		{
		}

		public ContinuableKeywordException(string message, Exception innerException) : base(message, innerException)
		{
		}

		// This constructor is needed for serialization.
		protected ContinuableKeywordException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}
}