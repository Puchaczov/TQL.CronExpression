using Cron.Parser.Enums;
using TQL.Core.Tokens;

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
