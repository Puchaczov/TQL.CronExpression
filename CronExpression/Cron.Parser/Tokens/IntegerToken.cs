using Cron.Core.Tokens;
using Cron.Parser.Enums;

namespace Cron.Parser.Tokens
{
    public class IntegerToken : Token
    {
        public IntegerToken(string number, TextSpan span)
            : base(number, Enums.TokenType.Integer, span)
        { }

        public override GenericToken<TokenType> Clone() => new IntegerToken(base.Value, Span.Clone());
    }
}
