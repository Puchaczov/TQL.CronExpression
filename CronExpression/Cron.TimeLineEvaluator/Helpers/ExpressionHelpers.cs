﻿using TQL.CronExpression.Extensions.TimelineEvaluator.Evaluators;
using TQL.CronExpression.Parser;
using TQL.CronExpression.Parser.Visitors;

namespace TQL.CronExpression.Extensions.TimelineEvaluator.Helpers
{
    public static class ExpressionHelpers
    {
        public static ICronFireTimeEvaluator TakeEvaluator(this string expression) => expression.TakeEvaluator<CronTimelineVisitor, ICronFireTimeEvaluator>(
    new CronTimelineVisitor());

        public static F TakeEvaluator<T, F>(this string expression, T visitor)
            where T : INodeVisitor, IEvaluable<F>
        {
            var lexer = new Lexer(expression);
            var parser = new CronParser(lexer);
            var nodes = parser.ComposeRootComponents();
            nodes.Accept(visitor);
            return visitor.Evaluator;
        }
    }
}
