using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cron.Parser.Tokens
{
    public class CommaToken : Token
    {
        public CommaToken()
            : base(",", Enums.TokenType.Comma)
        { }
    }
}
