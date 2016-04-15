using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cron.Parser.Tokens
{
    public class QuestionMarkToken : Token
    {
        public QuestionMarkToken(TextSpan span)
            : base("?", Enums.TokenType.QuestionMark, span)
        { }

        public override Token Clone() => new QuestionMarkToken(Span.Clone());
    }
}
