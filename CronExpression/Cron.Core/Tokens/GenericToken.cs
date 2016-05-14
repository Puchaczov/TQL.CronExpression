using System;

namespace Cron.Core.Tokens
{
    public abstract class GenericToken<TTokenType>
        where TTokenType : struct, IComparable, IFormattable
    {
        public TextSpan Span { get; }
        public TTokenType TokenType { get; }

        protected GenericToken(string value, TTokenType type, TextSpan span)
        {
            this.Value = value;
            this.TokenType = type;
            this.Span = span;
        }

        public string Value
        {
            get;
        }

        public abstract GenericToken<TTokenType> Clone();
    }
}
