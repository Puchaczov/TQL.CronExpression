using System;
using System.Runtime.Serialization;
using Cron.Visitors.Exceptions;

namespace Cron.Visitors.Exceptions
{
    [Serializable]
    public class RangeNodeException : Exception
    {
        private readonly BaseCronValidationException exc;

        public RangeNodeException()
        {
        }

        public RangeNodeException(string message) : base(message)
        {
        }

        public RangeNodeException(BaseCronValidationException exc)
            : base(exc.Message, exc)
        {
            this.exc = exc;
        }

        public RangeNodeException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected RangeNodeException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public override string StackTrace
        {
            get
            {
                return exc.StackTrace;
            }
        }

        public override string Message
        {
            get
            {
                return exc.Message;
            }
        }
    }
}