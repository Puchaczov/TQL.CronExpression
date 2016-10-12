using System.Diagnostics;
using TQL.Core.Tokens;
using TQL.CronExpression.Parser.Enums;

namespace TQL.CronExpression.Parser.Tokens
{
    [DebuggerDisplay("{GetType().Name,nq}: ,")]
    public class CommaToken : Token
    {
        public CommaToken(TextSpan span)
            : base(",", Enums.TokenType.Comma, span)
        { }

        public override GenericToken<TokenType> Clone() => new CommaToken(this.Span.Clone());
    }
}
