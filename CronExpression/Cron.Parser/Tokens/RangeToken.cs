using Cron.Core.Tokens;
using Cron.Parser.Enums;

namespace Cron.Parser.Tokens
{
    public class RangeToken : Token
    {
        public RangeToken(TextSpan span)
            : base("-", Enums.TokenType.Range, span)
        { }

        public override GenericToken<TokenType> Clone() => new RangeToken(Span.Clone());
    }
}
