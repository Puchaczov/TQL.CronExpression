using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cron.Parser.Tokens
{
    public class LToken : Token
    {
        public LToken()
            : base("L", Enums.TokenType.L)
        { }
    }
}
