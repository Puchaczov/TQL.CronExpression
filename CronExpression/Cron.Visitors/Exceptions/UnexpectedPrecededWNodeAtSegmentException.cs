using Cron.Parser.Enums;
using Cron.Parser.Tokens;

namespace Cron.Visitors.Exceptions
{
    public class UnexpectedPrecededWNodeAtSegmentException : BaseCronValidationException
    {
        private Segment segment;

        public UnexpectedPrecededWNodeAtSegmentException(Token token, Segment segment)
            : base(token)
        {
            this.segment = segment;
        }
    }
}