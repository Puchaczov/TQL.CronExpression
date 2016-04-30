using System;
using Cron.Parser.Enums;

namespace Cron.Parser.Exceptions
{
    public class UnexpectedSegmentException : Exception
    {
        private readonly Segment segment;

        public UnexpectedSegmentException(Segment segment)
        {
            this.segment = segment;
        }
    }
}
