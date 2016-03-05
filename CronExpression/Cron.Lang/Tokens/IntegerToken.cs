using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cron.Parser.Tokens
{
    public class IntegerToken : Token
    {
        public IntegerToken(string number, TextSpan span)
            : base(number, Enums.TokenType.Integer, span)
        { }

        public override Token Clone()
        {
            return new IntegerToken(base.Value, Span.Clone());
        }
    }
}
