using System.Diagnostics;
using TQL.Core.Tokens;
using TQL.CronExpression.Parser.Enums;

namespace TQL.CronExpression.Parser.Tokens
{
    [DebuggerDisplay("{GetType().Name,nq}: _")]
    public class MissingToken : Token
    {
        public MissingToken(TextSpan span)
            : base("_", Enums.TokenType.Missing, span)
        { }

        public override GenericToken<TokenType> Clone() => new MissingToken(Span.Clone());
    }
}
