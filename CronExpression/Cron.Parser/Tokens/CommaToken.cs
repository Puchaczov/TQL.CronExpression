using Cron.Parser.Enums;
using TQL.Core.Tokens;

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
