using Cron.Core.Tokens;
using Cron.Parser.Enums;

namespace Cron.Parser.Tokens
{
    public class IncrementByToken : Token
    {
        public IncrementByToken(TextSpan span)
            : base("/", Enums.TokenType.Inc, span)
        { }

        public override GenericToken<TokenType> Clone() => new IncrementByToken(Span.Clone());
    }
}
