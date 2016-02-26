using Cron.Parser.Enums;
using Cron.Parser.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cron.Parser.Exceptions
{
    public class ExpressionTooShortException : BaseCronValidationException
    {
        private Segment segment;

        public ExpressionTooShortException(Token token, Segment segment)
            : base(token)
        {
            this.segment = segment;
        }
    }
}
