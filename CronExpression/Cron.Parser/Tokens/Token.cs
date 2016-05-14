using System;
using Cron.Core.Tokens;
using Cron.Parser.Enums;

namespace Cron.Parser.Tokens
{
    public abstract class Token : GenericToken<TokenType>
    {
        public Token(string value, TokenType type, Core.Tokens.TextSpan span) : base(value, type, span)
        { }
    }
}
