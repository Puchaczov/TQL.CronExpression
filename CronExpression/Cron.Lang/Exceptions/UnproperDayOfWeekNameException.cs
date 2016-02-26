using Cron.Parser.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cron.Parser.Exceptions
{
    public class UnproperDayOfWeekException : BaseCronValidationException
    {
        public UnproperDayOfWeekException(Token token)
            : base(token)
        { }
    }
}
