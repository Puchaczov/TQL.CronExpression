using Cron.Parser.Enums;

namespace Cron.Parser.Tokens
{
    public abstract class Token
    {

        protected Token(string value, TokenType type, TextSpan span)
        {
            this.Value = value;
            this.TokenType = type;
            this.Span = span;
        }
        public TextSpan Span { get; }
        public TokenType TokenType { get; }

        public string Value
        {
            get;
        }

        public abstract Token Clone();
    }
}
