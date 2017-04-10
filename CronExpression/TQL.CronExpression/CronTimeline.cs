using System;
using System.Linq;
using TQL.Common.Evaluators;
using TQL.Common.Timezone;
using TQL.CronExpression.Exceptions;
using TQL.CronExpression.Parser.Nodes;
using TQL.CronExpression.TimelineEvaluator;
using TQL.CronExpression.TimelineEvaluator.Evaluators;
using TQL.CronExpression.Visitors;
using TQL.Interfaces;

namespace TQL.CronExpression
{
    public class CronTimeline : AbstractConverter<IFireTimeEvaluator>,
        IConverter<CreateEvaluatorRequest, ConvertionResponse<IFireTimeEvaluator>>
    {
        public CronTimeline(bool throwOnError = false)
            : base(throwOnError)
        { }

        public ConvertionResponse<IFireTimeEvaluator> Convert(CreateEvaluatorRequest request)
        {
            if (!request.Options.ProduceEndOfFileNode)
                throw new ArgumentException("Produce end of file node option must be turned on to evaluate expression");
            return base.Convert(request, ast => Convert(ast, request));
        }

        private ConvertionResponse<IFireTimeEvaluator> Convert(RootComponentNode ast, CreateEvaluatorRequest request)
        {
            var rulesVisitor = new CronRulesNodeVisitor();
            ast.Accept(rulesVisitor);
            if (ThrowOnError && rulesVisitor.Errors.Any(f => f.Level == MessageLevel.Error))
                throw new IncorrectCronExpressionException(rulesVisitor.Errors.ToArray());

            if (rulesVisitor.IsValid)
            {
                var timelineVisitor = new CronTimelineVisitor();
                ast.Accept(timelineVisitor);

                IFireTimeEvaluator evaluator = timelineVisitor.Evaluator;
                
                ((ICronFireTimeEvaluator)evaluator).ReferenceTime = new DateTimeOffset(request.ReferenceTime, request.Source.GetUtcOffset(request.ReferenceTime));

                evaluator = new TimeZoneAdjuster(request.Source, evaluator);
                evaluator = new DaylightSavingTimeTracker(request.Source, evaluator);

                return
                    new ConvertionResponse<IFireTimeEvaluator>(
                        new TimeZoneChangerDecorator(request.Source, request.Destination, evaluator),
                        rulesVisitor.Errors.ToArray());
            }
            return new ConvertionResponse<IFireTimeEvaluator>(null, rulesVisitor.Errors.ToArray());
        }

        protected override ConvertionResponse<IFireTimeEvaluator> GetErrorResponse(Exception exc)
            => new ConvertionResponse<IFireTimeEvaluator>(null, new FatalError(exc));
    }
}