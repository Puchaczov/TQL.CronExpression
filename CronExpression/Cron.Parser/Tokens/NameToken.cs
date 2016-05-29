using Cron.Parser.Enums;
using TQL.Core.Tokens;

namespace Cron.Parser.Tokens
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
