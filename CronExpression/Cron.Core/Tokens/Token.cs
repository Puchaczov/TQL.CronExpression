using System;

namespace Cron.Core.Tokens
{
    public abstract class GenericToken<TokenType> where TokenType: struct, IComparable, IFormattable
    {
        public TextSpan Span { get; }
        public TokenType Type { get; }

        protected GenericToken(string value, TokenType type, TextSpan span)
        {
            this.Value = value;
            this.Type = type;
            this.Span = span;
        }

        public string Value
        {
            get;
        }

        public abstract GenericToken<TokenType> Clone();
    }
}
