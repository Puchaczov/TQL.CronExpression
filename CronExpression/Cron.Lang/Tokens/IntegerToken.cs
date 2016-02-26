using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cron.Parser.Tokens
{
    public class IntegerToken : Token
    {
        public IntegerToken(string number)
            : base(number, Enums.TokenType.Integer)
        { }
    }
}
