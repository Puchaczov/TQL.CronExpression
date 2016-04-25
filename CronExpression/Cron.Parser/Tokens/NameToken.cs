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

        public int Length => base.Value.Length;

        public override Token Clone() => new NameToken(base.Value, Span.Clone());
    }
}
