using System.Diagnostics;
using TQL.Core.Tokens;
using TQL.CronExpression.Parser.Enums;

namespace TQL.CronExpression.Parser.Tokens
{
    [DebuggerDisplay("{GetType().Name,nq}: {Value,nq}W")]
    public class WToken : Token
    {
        public WToken(int value, TextSpan span)
            : base(value.ToString(), Enums.TokenType.W, span)
        {
            Number = value;
        }

        public int Number { get; }

        public override GenericToken<TokenType> Clone() => new WToken(Number, Span.Clone());
    }
}