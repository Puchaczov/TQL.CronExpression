using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cron.Parser.Tokens
{
    public class RangeToken : Token
    {
        public RangeToken(TextSpan span)
            : base("-", Enums.TokenType.Range, span)
        { }

        public override Token Clone()
        {
            return new RangeToken(Span.Clone());
        }
    }
}
