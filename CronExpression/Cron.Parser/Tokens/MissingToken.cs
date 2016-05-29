using Cron.Parser.Enums;
using TQL.Core.Tokens;

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
