using Cron.Parser.Enums;
using TQL.Core.Tokens;

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
