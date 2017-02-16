using System;

namespace TQL.CronExpression.Visitors.Exceptions
{
    public class RangeNodeException : Exception
    {
        private readonly BaseCronValidationException _exc;

        public RangeNodeException(BaseCronValidationException exc)
        {
            this._exc = exc;
        }
    }
}