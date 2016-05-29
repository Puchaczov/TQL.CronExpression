using Cron.Parser.Enums;
using TQL.Core.Tokens;

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
