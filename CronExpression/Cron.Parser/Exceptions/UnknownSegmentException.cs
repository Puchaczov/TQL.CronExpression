using System;
using System.Runtime.Serialization;

namespace Cron.Parser.Exceptions
{
    [Serializable]
    public class UnknownSegmentException : Exception
    {
        private readonly int position;

        public UnknownSegmentException(int position)
        {
            this.position = position;
        }

        protected UnknownSegmentException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public override string Message => $"Unknown segment while processing expression. Exception occured in position {Position}";

        public int Position => position;
    }
}