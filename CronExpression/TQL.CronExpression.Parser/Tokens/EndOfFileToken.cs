using System.Diagnostics;
using TQL.Core.Tokens;
using TQL.CronExpression.Parser.Enums;

namespace TQL.CronExpression.Parser.Tokens
{
    [DebuggerDisplay("{GetType().Name,nq}: EOF")]
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
