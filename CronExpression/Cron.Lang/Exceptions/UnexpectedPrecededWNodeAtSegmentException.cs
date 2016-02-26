using System;
using System.Runtime.Serialization;
using Cron.Parser.Enums;

namespace Cron.Parser.Exceptions
{
    [Serializable]
    public class UnexpectedPrecededWNodeAtSegmentException : Exception
    {
        private Segment segment;

        public UnexpectedPrecededWNodeAtSegmentException()
        {
        }

        public UnexpectedPrecededWNodeAtSegmentException(string message) : base(message)
        {
        }

        public UnexpectedPrecededWNodeAtSegmentException(Segment segment)
        {
            this.segment = segment;
        }

        public UnexpectedPrecededWNodeAtSegmentException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected UnexpectedPrecededWNodeAtSegmentException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}