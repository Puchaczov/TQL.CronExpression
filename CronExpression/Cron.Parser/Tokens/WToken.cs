using Cron.Parser.Enums;
using TQL.Core.Tokens;

namespace Cron.Parser.Tokens
{
    public class WToken : Token
    {
        public WToken(TextSpan span)
            : base("W", Enums.TokenType.W, span)
        { }

        public override GenericToken<TokenType> Clone() => new WToken(Span.Clone());
    }
}
