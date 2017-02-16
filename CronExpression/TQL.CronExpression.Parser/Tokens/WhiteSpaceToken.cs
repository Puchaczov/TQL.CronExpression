using System.Diagnostics;
using TQL.Core.Tokens;
using TQL.CronExpression.Parser.Enums;

namespace TQL.CronExpression.Parser.Tokens
{
    [DebuggerDisplay("{GetType().Name,nq}: ' '")]
    public class WhiteSpaceToken : Token
    {
        public WhiteSpaceToken(TextSpan span)
            : base(" ", Enums.TokenType.WhiteSpace, span)
        {
        }

        public override GenericToken<TokenType> Clone() => new WhiteSpaceToken(Span.Clone());
    }
}