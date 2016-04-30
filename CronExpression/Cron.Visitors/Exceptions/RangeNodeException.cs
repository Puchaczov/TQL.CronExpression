using System;

namespace Cron.Visitors.Exceptions
{
    public class RangeNodeException : Exception
    {
        private readonly BaseCronValidationException exc;

        public RangeNodeException(BaseCronValidationException exc)
        {
            this.exc = exc;
        }
    }
}
