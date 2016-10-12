using System.Diagnostics;
using TQL.Core.Tokens;
using TQL.CronExpression.Parser.Enums;

namespace TQL.CronExpression.Parser.Tokens
{
    [DebuggerDisplay("{GetType().Name,nq}: {Value,nq}LW")]
    public class LWToken : Token
    {
        public LWToken(int value, TextSpan span)
            : base(value.ToString(), Enums.TokenType.LW, span)
        {
            this.Number = value;
        }

        public int Number
        {
            get;
        }

        public override GenericToken<TokenType> Clone() => new LWToken(Number, Span.Clone());
    }
}
