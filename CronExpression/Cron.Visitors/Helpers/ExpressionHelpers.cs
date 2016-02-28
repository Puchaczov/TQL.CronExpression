﻿using Cron.Parser;
using Cron.Parser.Visitors;
using Cron.Visitors.Evaluators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cron.Visitors.Helpers
{
    public static class ExpressionHelpers
    {
        public static ICronFireTimeEvaluator TakeEvaluator(this string expression)
        {
            return expression.TakeEvaluator<CronTimelineVisitor, ICronFireTimeEvaluator>(
                new CronTimelineVisitor());
        }

        public static F TakeEvaluator<T, F>(this string expression, T visitor)
            where T : INodeVisitor, IEvaluable<F>
        {
            Lexer lexer = new Lexer(expression);
            CronParser parser = new CronParser(lexer);
            var nodes = parser.ComposeRootComponents();
            nodes.Accept(visitor);
            return visitor.Evaluator;
        }
    }
}
