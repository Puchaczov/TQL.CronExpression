using Cron.Parser.Enums;
using Cron.Parser.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cron.Visitors.Exceptions
{
    public class UnexpectedLNodeAtSegment : BaseCronValidationException
    {
        private readonly Segment segment;

        public UnexpectedLNodeAtSegment(Token token, Segment segment)
            : base(token)
        {
            this.segment = segment;
        }
    }
}
