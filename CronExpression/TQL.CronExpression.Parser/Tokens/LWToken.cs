using System.Diagnostics;
using TQL.Core.Tokens;
using TQL.CronExpression.Parser.Enums;

namespace TQL.CronExpression.Parser.Tokens
{
    [DebuggerDisplay("{GetType().Name,nq}: {Value,nq}LW")]
    public class LwToken : Token
    {
        public LwToken(int value, TextSpan span)
            : base(value.ToString(), Enums.TokenType.Lw, span)
        {
            Number = value;
        }

        public int Number { get; }

        public override GenericToken<TokenType> Clone() => new LwToken(Number, Span.Clone());
    }
}