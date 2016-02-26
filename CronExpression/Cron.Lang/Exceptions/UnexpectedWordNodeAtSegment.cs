using Cron.Parser.Enums;
using Cron.Parser.Tokens;

namespace Cron.Parser.Exceptions
{
    public class UnexpectedWordNodeAtSegment : BaseCronValidationException
    {
        private Segment segment;

        public UnexpectedWordNodeAtSegment(Token token, Segment segment)
            : base(token)
        {
            this.segment = segment;
        }
    }
}
