using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cron.Parser.Tokens
{
    public class EndOfFileToken : Token
    {
        public EndOfFileToken(TextSpan span)
            :base(string.Empty, Enums.TokenType.Eof, span)
        { }

        public override Token Clone()
        {
            return new EndOfFileToken(Span.Clone());
        }
    }
}
