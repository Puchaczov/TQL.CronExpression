using Cron.Parser.Nodes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cron.Parser.Helpers
{
    public static partial class ExpressionHelpers
    {
        public static RootComponentNode Parse(this string expression, bool produceMissingYearComponent = true, bool produceEndOfFileNodeComponent = true)
        {
            var lexer = new Lexer(expression);
            var parser = new CronParser(lexer, produceMissingYearComponent, produceEndOfFileNodeComponent);
            return parser.ComposeRootComponents();
        }
    }
}
