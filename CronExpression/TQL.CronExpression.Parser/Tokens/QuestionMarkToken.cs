using System.Diagnostics;
using TQL.Core.Tokens;
using TQL.CronExpression.Parser.Enums;

namespace TQL.CronExpression.Parser.Tokens
{
    [DebuggerDisplay("{GetType().Name,nq}: ?")]
    public class QuestionMarkToken : Token
    {
        public QuestionMarkToken(TextSpan span)
            : base("?", Enums.TokenType.QuestionMark, span)
        {
        }

        public override GenericToken<TokenType> Clone() => new QuestionMarkToken(Span.Clone());
    }
}