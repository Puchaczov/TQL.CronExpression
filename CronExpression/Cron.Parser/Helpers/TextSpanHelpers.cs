using Cron.Parser.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cron.Parser.Helpers
{
    public static class TextSpanHelpers
    {

        public static bool IsEqual(this TextSpan span1, TextSpan span2)
        {
            if (span1.Start == span2.Start && span1.End == span2.End)
            {
                return true;
            }
            return false;
        }
        public static bool IsInside(this TextSpan span1, TextSpan span2)
        {
            if (span1.Start >= span2.Start && span1.End <= span2.End)
            {
                return true;
            }
            return false;
        }
    }
}
