using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cron.Parser.Tokens
{
    public class WhiteSpaceToken : Token
    {
        public WhiteSpaceToken()
            : base(" ", Enums.TokenType.WhiteSpace)
        { }
    }
}
