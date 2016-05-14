using Cron.Core.Tokens;
using Cron.Parser.Enums;

namespace Cron.Parser.Tokens
{
    public class LWToken : Token
    {
        public LWToken(TextSpan span)
            : base("LW", Enums.TokenType.LW, span)
        { }

        public override GenericToken<TokenType> Clone() => new LWToken(Span.Clone());
    }
}
