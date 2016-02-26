using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cron.Parser.Tokens
{
    public class StarToken : Token
    {
        public StarToken()
            : base("*", Enums.TokenType.Star)
        { }
    }
}
