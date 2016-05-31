using TQL.Core.Tokens;
using TQL.CronExpression.Parser.Enums;

namespace TQL.CronExpression.Parser.Tokens
{
    public class NameToken : Token
    {
        public NameToken(string word, TextSpan span)
            : base(word, Enums.TokenType.Name, span)
        { }

        public int Length => base.Value.Length;

        public override GenericToken<TokenType> Clone() => new NameToken(base.Value, Span.Clone());
    }
}
