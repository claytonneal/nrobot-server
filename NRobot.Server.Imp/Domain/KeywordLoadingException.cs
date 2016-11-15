using System;

namespace NRobot.Server.Imp.Domain
{
    public class KeywordLoadingException : Exception
    {

        public KeywordLoadingException() { }

        public KeywordLoadingException(string message) : base(message) { }

        public KeywordLoadingException(string message, Exception inner) : base(message, inner) { }

    }
}
