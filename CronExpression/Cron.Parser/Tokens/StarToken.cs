using Cron.Core.Tokens;
using Cron.Parser.Enums;

namespace Cron.Parser.Tokens
{
    public class StarToken : Token
    {
        public StarToken(TextSpan span)
            : base("*", Enums.TokenType.Star, span)
        { }

        public override GenericToken<TokenType> Clone() => new StarToken(Span.Clone());
    }
}
