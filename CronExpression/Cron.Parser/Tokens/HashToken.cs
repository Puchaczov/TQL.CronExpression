using Cron.Core.Tokens;
using Cron.Parser.Enums;

namespace Cron.Parser.Tokens
{
    public class HashToken : Token
    {
        public HashToken(TextSpan span)
            : base("#", Enums.TokenType.Hash, span)
        { }

        public override GenericToken<TokenType> Clone() => new HashToken(Span.Clone());
    }
}
