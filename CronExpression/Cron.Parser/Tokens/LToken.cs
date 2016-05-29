using Cron.Parser.Enums;
using TQL.Core.Tokens;

namespace Cron.Parser.Tokens
{
    public class LToken : Token
    {
        public LToken(TextSpan span)
            : base("L", Enums.TokenType.L, span)
        { }

        public override GenericToken<TokenType> Clone() => new LToken(Span.Clone());
    }
}
