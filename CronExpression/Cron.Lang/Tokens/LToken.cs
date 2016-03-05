using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cron.Parser.Tokens
{
    public class LToken : Token
    {
        public LToken(TextSpan span)
            : base("L", Enums.TokenType.L, span)
        { }

        public override Token Clone()
        {
            return new LToken(Span.Clone());
        }
    }
}
