using TQL.Core.Tokens;
using TQL.CronExpression.Parser.Enums;

namespace TQL.CronExpression.Parser.Tokens
{
    public class LWToken : Token
    {
        public LWToken(TextSpan span)
            : base("LW", Enums.TokenType.LW, span)
        { }

        public override GenericToken<TokenType> Clone() => new LWToken(Span.Clone());
    }
}
