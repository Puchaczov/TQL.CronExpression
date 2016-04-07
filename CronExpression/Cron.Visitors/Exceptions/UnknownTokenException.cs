using System;
using System.Runtime.Serialization;

namespace Cron.Parser.Exceptions
{
    [Serializable]
    public class UnknownTokenException : Exception
    {
        private readonly char currentChar;
        private readonly int pos;

        public UnknownTokenException(int pos, char currentChar)
        {
            this.pos = pos;
            this.currentChar = currentChar;
        }

        protected UnknownTokenException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}