using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cron.Parser.Enums
{
    public enum TokenType
    {
        Integer,
        DoW,
        Hash,
        W,
        L,
        QuestionMark,
        Comma,
        Star,
        Eof,
        WhiteSpace,
        Range,
        LeftRange,
        RightRange,
        Inc,
        Name,
        None,
        LW,
        Missing
    }
}
