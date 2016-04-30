using Cron.Exceptions;
using Cron.Extensions.TimelineEvaluator;
using Cron.Extensions.TimelineEvaluator.Evaluators;
using Cron.Parser.Nodes;
using Cron.Visitors;
using System;
using System.Linq;

namespace Cron
{
    public class CronTimeline : CronValidator
    {
        public CronTimeline(bool throwOnError = false)
            : base(throwOnError)
        { }

        public ConvertionResponse<ICronFireTimeEvaluator> GetEvaluator(ConvertionRequest request)
        {
            if(!request.Options.ProduceEndOfFileNode)
            {
                throw new ArgumentException("Produce end of file node option must be turned on to evaluate expression");
            }
            return base.Convert(request, Convert);
        }

        private static ConvertionResponse<ICronFireTimeEvaluator> Convert(RootComponentNode ast)
        {
            var visitor = new CronTimelineVisitor();
            ast.Accept(visitor);
            if (visitor.Errors.Any(f => f.Level == MessageLevel.Error))
            {
                throw new IncorrectCronExpressionException(visitor.Errors.ToArray());
            }
            return new ConvertionResponse<ICronFireTimeEvaluator>(visitor.Errors.Count() == 0 ? visitor.Evaluator : null, visitor.Errors.ToArray());
        }
    }
}
