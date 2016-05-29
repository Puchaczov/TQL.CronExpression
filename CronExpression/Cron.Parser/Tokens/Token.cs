using Cron.Parser.Enums;
using TQL.Core.Tokens;

namespace Cron.Parser.Tokens
{
    public abstract class Token : GenericToken<TokenType>
    {
        public Token(string value, TokenType type, TextSpan span) : base(value, type, span)
        { }
    }
}
