using TQL.Core.Tokens;
using TQL.CronExpression.Parser.Enums;

namespace TQL.CronExpression.Parser.Tokens
{
    public class LToken : Token
    {
        public LToken(TextSpan span)
            : base("L", Enums.TokenType.L, span)
        { }

        public override GenericToken<TokenType> Clone() => new LToken(Span.Clone());
    }
}
