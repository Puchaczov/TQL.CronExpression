using System.Diagnostics;
using TQL.Core.Tokens;
using TQL.CronExpression.Parser.Enums;

namespace TQL.CronExpression.Parser.Tokens
{
    [DebuggerDisplay("{GetType().Name,nq}: {Value,nq}")]
    public class NameToken : Token
    {
        public NameToken(string word, TextSpan span)
            : base(word, Enums.TokenType.Name, span)
        {
        }

        public int Length => Value.Length;

        public override GenericToken<TokenType> Clone() => new NameToken(Value, Span.Clone());
    }
}