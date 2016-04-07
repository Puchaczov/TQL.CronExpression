using System;
using System.Runtime.Serialization;

namespace Cron.Parser.Exceptions
{
    [Serializable]
    public class UnknownSegmentException : Exception
    {
        private readonly int position;

        public int Position
        {
            get
            {
                return position;
            }
        }

        public UnknownSegmentException(int position)
        {
            this.position = position;
        }

        protected UnknownSegmentException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public override string Message
        {
            get
            {
                return $"Unknown segment while processing expression. Exception occured in position {Position}";
            }
        }
    }
}