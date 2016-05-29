using Cron.Parser.Enums;
using TQL.Core.Tokens;

namespace Cron.Parser.Tokens
{
    public class QuestionMarkToken : Token
    {
        public QuestionMarkToken(TextSpan span)
            : base("?", Enums.TokenType.QuestionMark, span)
        { }

        public override GenericToken<TokenType> Clone() => new QuestionMarkToken(Span.Clone());
    }
}
