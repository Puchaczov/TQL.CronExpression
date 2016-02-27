using Cron.Parser.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cron.Visitors.Exceptions
{
    public class UnrecognizedDayOfWeekException : BaseCronValidationException
    {
        public UnrecognizedDayOfWeekException(Token token)
            : base(token)
        { }
    }
}
