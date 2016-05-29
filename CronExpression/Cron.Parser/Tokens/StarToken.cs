using Cron.Parser.Enums;
using TQL.Core.Tokens;

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
