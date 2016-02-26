using Cron.Parser.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cron.Parser.Exceptions
{
    public class UnproperMonthNameException : BaseCronValidationException
    {
        public UnproperMonthNameException(Token token)
            : base(token)
        { }
    }
}
