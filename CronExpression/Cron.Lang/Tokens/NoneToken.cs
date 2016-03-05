using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cron.Parser.Tokens
{
    public class NoneToken : Token
    {
        public NoneToken(TextSpan span)
            : base(string.Empty, Enums.TokenType.None, span)
        { }

        public override Token Clone()
        {
            return new NoneToken(Span.Clone());
        }
    }
}
