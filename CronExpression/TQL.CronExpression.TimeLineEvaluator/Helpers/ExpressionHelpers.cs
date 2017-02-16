using TQL.CronExpression.Parser;
using TQL.CronExpression.TimelineEvaluator.Evaluators;

namespace TQL.CronExpression.TimelineEvaluator.Helpers
{
    public static class ExpressionHelpers
    {
        public static ICronFireTimeEvaluator TakeEvaluator(this string expression)
            => expression.TakeEvaluator<CronTimelineVisitor, ICronFireTimeEvaluator>(
                new CronTimelineVisitor());

        public static TF TakeEvaluator<T, TF>(this string expression, T visitor)
            where T : INodeVisitor, IEvaluable<TF>
        {
            var lexer = new Lexer(expression);
            var parser = new CronParser(lexer);
            var nodes = parser.ComposeRootComponents();
            nodes.Accept(visitor);
            return visitor.Evaluator;
        }
    }
}