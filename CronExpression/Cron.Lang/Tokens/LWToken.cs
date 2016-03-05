using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cron.Parser.Tokens
{
    public class LWToken : Token
    {
        public LWToken(TextSpan span)
            : base("LW", Enums.TokenType.LW, span)
        { }

        public override Token Clone()
        {
            return new LWToken(Span.Clone());
        }
    }
}
