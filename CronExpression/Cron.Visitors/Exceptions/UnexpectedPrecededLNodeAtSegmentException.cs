using System;
using Cron.Parser.Enums;

namespace Cron.Visitors.Exceptions
{
    internal class UnexpectedPrecededLNodeAtSegmentException : Exception
    {
        private Segment segment;

        public UnexpectedPrecededLNodeAtSegmentException()
        {
        }

        public UnexpectedPrecededLNodeAtSegmentException(string message) : base(message)
        {
        }

        public UnexpectedPrecededLNodeAtSegmentException(Segment segment)
        {
            this.segment = segment;
        }
    }
}