using System.Diagnostics;
using TQL.Core.Tokens;
using TQL.CronExpression.Parser.Enums;

namespace TQL.CronExpression.Parser.Tokens
{
    [DebuggerDisplay("{GetType().Name,nq}: {Value,nq}L")]
    public class LToken : Token
    {
        public LToken(int value, TextSpan span)
            : base(value.ToString(), Enums.TokenType.L, span)
        {
            this.Number = value;
        }

        public int Number
        {
            get;
        }

        public override GenericToken<TokenType> Clone() => new LToken(Number, Span.Clone());
    }
}
