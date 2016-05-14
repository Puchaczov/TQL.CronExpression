using Cron.Core.Tokens;
using Cron.Parser.Enums;

namespace Cron.Parser.Tokens
{
    public class MissingToken : Token
    {
        public MissingToken(TextSpan span)
            : base("_", Enums.TokenType.Missing, span)
        { }

        public override GenericToken<TokenType> Clone() => new MissingToken(Span.Clone());
    }
}
