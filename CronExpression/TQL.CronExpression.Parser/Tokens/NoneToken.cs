using TQL.Core.Tokens;
using TQL.CronExpression.Parser.Enums;

namespace TQL.CronExpression.Parser.Tokens
{
    public class NoneToken : Token
    {
        public NoneToken(TextSpan span)
            : base(string.Empty, Enums.TokenType.None, span)
        {
        }

        public override GenericToken<TokenType> Clone() => new NoneToken(Span.Clone());
    }
}