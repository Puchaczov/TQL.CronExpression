using Cron.Parser.Enums;
using TQL.Core.Tokens;

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
