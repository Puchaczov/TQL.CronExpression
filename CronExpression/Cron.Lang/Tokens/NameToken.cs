using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cron.Parser.Tokens
{
    public class NameToken : Token
    {
        public NameToken(string word)
            : base(word, Enums.TokenType.Name)
        { }

        public int Length
        {
            get
            {
                return base.Value.Count();
            }
        }
    }
}
