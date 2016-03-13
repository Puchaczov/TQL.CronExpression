using System;

namespace Cron.Visitors.Exceptions
{
    public class RangeNodeException : Exception
    {
        private BaseCronValidationException exc;

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