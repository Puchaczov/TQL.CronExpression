using System;
using System.Runtime.Serialization;
using Cron.Parser.Enums;

namespace Cron.Visitors.Exceptions
{
    [Serializable]
    internal class UnexpectedPrecededLNodeAtSegmentException : Exception
    {
        private readonly Segment segment;

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

        public UnexpectedPrecededLNodeAtSegmentException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected UnexpectedPrecededLNodeAtSegmentException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}