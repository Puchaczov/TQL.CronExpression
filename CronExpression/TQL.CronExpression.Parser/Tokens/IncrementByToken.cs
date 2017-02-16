using System.Diagnostics;
using TQL.Core.Tokens;
using TQL.CronExpression.Parser.Enums;

namespace TQL.CronExpression.Parser.Tokens
{
    [DebuggerDisplay("{GetType().Name,nq}: /")]
    public class IncrementByToken : Token
    {
        public IncrementByToken(TextSpan span)
            : base("/", Enums.TokenType.Inc, span)
        {
        }

        public override GenericToken<TokenType> Clone() => new IncrementByToken(Span.Clone());
    }
}