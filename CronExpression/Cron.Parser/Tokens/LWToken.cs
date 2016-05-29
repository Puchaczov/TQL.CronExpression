using Cron.Parser.Enums;
using TQL.Core.Tokens;

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
