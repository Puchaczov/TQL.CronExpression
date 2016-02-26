using System;
using System.Runtime.Serialization;

namespace Cron.Parser
{
    [Serializable]
    internal class UnknownSegmentException : Exception
    {
        private int position;

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
                return string.Format("Unknown segment while processing expression. Exception occured in position {0}", Position);
            }
        }
    }
}