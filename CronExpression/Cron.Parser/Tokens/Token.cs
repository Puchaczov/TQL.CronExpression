using TQL.Core.Tokens;
using TQL.CronExpression.Parser.Enums;

namespace TQL.CronExpression.Parser.Tokens
{
    public abstract class Token : GenericToken<TokenType>
    {
        public Token(string value, TokenType type, TextSpan span) : base(value, type, span)
        { }
    }
}
