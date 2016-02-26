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
        public TokenType TokenType { get; private set; }

        public string Value
        {
            get;
            private set;
        }

        public Token(string value, TokenType type)
        {
            this.Value = value;
            this.TokenType = type;
        }
    }
}
