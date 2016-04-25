using System;
using System.Runtime.Serialization;

namespace Cron.Parser.Exceptions
{
    public class UnknownTokenException : Exception
    {
        private readonly char currentChar;
        private readonly int pos;

        public UnknownTokenException(int pos, char currentChar)
        {
            this.pos = pos;
            this.currentChar = currentChar;
        }
    }
}