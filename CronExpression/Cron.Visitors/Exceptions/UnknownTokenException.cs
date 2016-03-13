using System;

namespace Cron.Parser.Exceptions
{
    public class UnknownTokenException : Exception
    {
        private char currentChar;
        private int pos;

        public UnknownTokenException(int pos, char currentChar)
        {
            this.pos = pos;
            this.currentChar = currentChar;
        }
    }
}