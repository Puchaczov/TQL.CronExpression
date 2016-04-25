using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
