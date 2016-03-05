using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cron.Parser.Tokens
{
    public class NameToken : Token
    {
        public NameToken(string word, TextSpan span)
            : base(word, Enums.TokenType.Name, span)
        { }

        public int Length
        {
            get
            {
                return base.Value.Count();
            }
        }

        public override Token Clone()
        {
            return new NameToken(base.Value, Span.Clone());
        }
    }
}
