using Cron.Parser.Tokens;

namespace Cron.Visitors.Exceptions
{
    public class InvertedRangeException : BaseCronValidationException
    {
        public InvertedRangeException(Token token)
            : base(token)
        { }
    }
}