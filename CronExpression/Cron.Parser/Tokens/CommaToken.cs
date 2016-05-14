using Cron.Core.Tokens;
using Cron.Parser.Enums;

namespace Cron.Parser.Tokens
{
    public class CommaToken : Token
    {
        public CommaToken(TextSpan span)
            : base(",", Enums.TokenType.Comma, span)
        { }

        public override GenericToken<TokenType> Clone() => new CommaToken(this.Span.Clone());
    }
}
