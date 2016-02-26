using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cron.Parser.Tokens
{
    public class WToken : Token
    {
        public WToken()
            : base("W", Enums.TokenType.W)
        { }
    }
}
