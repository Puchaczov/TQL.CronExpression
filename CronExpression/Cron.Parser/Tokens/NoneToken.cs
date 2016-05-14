using Cron.Core.Tokens;
using Cron.Parser.Enums;

namespace Cron.Parser.Tokens
{
    public class NoneToken : Token
    {
        public NoneToken(TextSpan span)
            : base(string.Empty, Enums.TokenType.None, span)
        { }

        public override GenericToken<TokenType> Clone() => new NoneToken(Span.Clone());
    }
}
