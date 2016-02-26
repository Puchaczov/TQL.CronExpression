using Cron.Parser.Enums;
using Cron.Parser.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cron.Visitors.Exceptions
{
    public class UnexpectedHashNodeAtSegment : BaseCronValidationException
    {
        public UnexpectedHashNodeAtSegment(Token token, Segment segment)
            : base(token)
        { }
    }
}
