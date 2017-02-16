using System.Diagnostics;
using TQL.Core.Tokens;
using TQL.CronExpression.Parser.Enums;

namespace TQL.CronExpression.Parser.Tokens
{
    [DebuggerDisplay("{GetType().Name,nq}: *")]
    public class StarToken : Token
    {
        public StarToken(TextSpan span)
            : base("*", Enums.TokenType.Star, span)
        {
        }

        public override GenericToken<TokenType> Clone() => new StarToken(Span.Clone());
    }
}