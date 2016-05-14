using Cron.Converter;
using Cron.Exceptions;
using Cron.Extensions.TimelineEvaluator;
using Cron.Extensions.TimelineEvaluator.Evaluators;
using Cron.Parser.Nodes;
using Cron.Visitors;
using System;
using System.Linq;

namespace Cron.Converter
{
    public class CronTimeline : AbstractConverter<ICronFireTimeEvaluator>, IConverter<CreateEvaluatorRequest, ConvertionResponse<ICronFireTimeEvaluator>>
    {
        public CronTimeline(bool throwOnError = false)
            : base(throwOnError)
        { }

        private ConvertionResponse<ICronFireTimeEvaluator> Convert(RootComponentNode ast, CreateEvaluatorRequest request)
        {
            var visitor = new CronTimelineVisitor();
            ast.Accept(visitor);
            if (throwOnError && visitor.Errors.Any(f => f.Level == MessageLevel.Error))
            {
                throw new IncorrectCronExpressionException(visitor.Errors.ToArray());
            }
            var evaluator = visitor.Evaluator;
            if(evaluator != null)
            {
                evaluator.ReferenceTime = request.ReferenceTime;
                evaluator = new TimeZoneCronForwardFireTimeEvaluatorDecorator(request.TargetTimeZoneInfo, evaluator);
                return new ConvertionResponse<ICronFireTimeEvaluator>(visitor.Errors.Count() == 0 ? evaluator : null, visitor.Errors.ToArray());
            }
            return new ConvertionResponse<ICronFireTimeEvaluator>(null, visitor.Errors.ToArray());
        }

        public ConvertionResponse<ICronFireTimeEvaluator> Convert(CreateEvaluatorRequest request)
        {
            if (!request.Options.ProduceEndOfFileNode)
            {
                throw new ArgumentException("Produce end of file node option must be turned on to evaluate expression");
            }
            return base.Convert(request, (ast) => this.Convert(ast, request));
        }
    }
}
