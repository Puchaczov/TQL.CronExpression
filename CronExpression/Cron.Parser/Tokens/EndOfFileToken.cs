using Cron.Core.Tokens;
using Cron.Parser.Enums;

namespace Cron.Parser.Tokens
{
    public class EndOfFileToken : Token
    {
        protected EndOfFileToken(string value, TokenType type, TextSpan span) 
            : base(value, type, span)
        { }

        public EndOfFileToken(TextSpan span)
            : base(string.Empty, TokenType.Eof, span)
        { }

        public override GenericToken<TokenType> Clone() => new EndOfFileToken(Value, TokenType.Eof, Span);
    }
}
