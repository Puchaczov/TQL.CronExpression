using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cron.Parser.Tokens
{
    public class HashToken : Token
    {
        public HashToken(TextSpan span)
            : base("#", Enums.TokenType.Hash, span)
        { }

        public override Token Clone() => new HashToken(Span.Clone());
    }
}
