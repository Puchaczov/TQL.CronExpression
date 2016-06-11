using TQL.Core.Tokens;
using TQL.CronExpression.Parser.Enums;

namespace TQL.CronExpression.Parser.Tokens
{
    public class WToken : Token
    {
        public WToken(TextSpan span)
            : base("W", Enums.TokenType.W, span)
        { }

        public override GenericToken<TokenType> Clone() => new WToken(Span.Clone());
    }
}
