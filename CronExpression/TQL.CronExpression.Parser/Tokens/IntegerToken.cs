using System.Diagnostics;
using TQL.Core.Tokens;
using TQL.CronExpression.Parser.Enums;

namespace TQL.CronExpression.Parser.Tokens
{
    [DebuggerDisplay("{GetType().Name,nq}: {Value,nq}")]
    public class IntegerToken : Token
    {
        public IntegerToken(string number, TextSpan span)
            : base(number, Enums.TokenType.Integer, span)
        {
        }

        public override GenericToken<TokenType> Clone() => new IntegerToken(Value, Span.Clone());
    }
}