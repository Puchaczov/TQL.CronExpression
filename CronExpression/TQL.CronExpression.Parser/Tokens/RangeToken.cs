using System.Diagnostics;
using TQL.Core.Tokens;
using TQL.CronExpression.Parser.Enums;

namespace TQL.CronExpression.Parser.Tokens
{
    [DebuggerDisplay("{GetType().Name,nq}: -")]
    public class RangeToken : Token
    {
        public RangeToken(TextSpan span)
            : base("-", Enums.TokenType.Range, span)
        {
        }

        public override GenericToken<TokenType> Clone() => new RangeToken(Span.Clone());
    }
}