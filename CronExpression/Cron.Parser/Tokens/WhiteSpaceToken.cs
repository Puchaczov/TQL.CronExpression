using Cron.Core.Tokens;
using Cron.Parser.Enums;

namespace Cron.Parser.Tokens
{
    public class WhiteSpaceToken : Token
    {
        public WhiteSpaceToken(TextSpan span)
            : base(" ", Enums.TokenType.WhiteSpace, span)
        { }

        public override GenericToken<TokenType> Clone() => new WhiteSpaceToken(Span.Clone());
    }
}
