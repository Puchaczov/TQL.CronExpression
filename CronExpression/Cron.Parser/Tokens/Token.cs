using Cron.Parser.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            private set;
        }

        public abstract Token Clone();
    }
}
