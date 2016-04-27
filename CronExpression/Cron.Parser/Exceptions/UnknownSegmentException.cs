using System;

namespace Cron.Parser.Exceptions
{
    public class UnknownSegmentException : Exception
    {
        private readonly int v;

        public UnknownSegmentException(int v)
        {
            this.v = v;
        }
    }
}
