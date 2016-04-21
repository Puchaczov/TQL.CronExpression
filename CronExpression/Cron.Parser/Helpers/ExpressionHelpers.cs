using Cron.Parser.Nodes;

namespace Cron.Parser.Helpers
{
    public static partial class ExpressionHelpers
    {
        public static RootComponentNode Parse(this string expression, bool produceMissingYearComponent = true, bool produceEndOfFileNodeComponent = true, bool produceMissingSecondComponent = false)
        {
            var lexer = new Lexer(expression);
            var parser = new CronParser(lexer, produceMissingYearComponent, produceEndOfFileNodeComponent, produceMissingSecondComponent);
            return parser.ComposeRootComponents();
        }
    }
}
