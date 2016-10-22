using System;
using System.Linq;
using TQL.CronExpression.Exceptions;
using TQL.CronExpression.TimelineEvaluator;
using TQL.CronExpression.TimelineEvaluator.Evaluators;
using TQL.CronExpression.Parser.Nodes;
using TQL.CronExpression.Visitors;

namespace TQL.CronExpression.Converter
{
    public class CronTimeline : AbstractConverter<ICronFireTimeEvaluator>, IConverter<CreateEvaluatorRequest, ConvertionResponse<ICronFireTimeEvaluator>>
    {
        public CronTimeline(bool throwOnError = false)
            : base(throwOnError)
        { }

        private ConvertionResponse<ICronFireTimeEvaluator> Convert(RootComponentNode ast, CreateEvaluatorRequest request)
        {
            var rulesVisitor = new CronRulesNodeVisitor();
            ast.Accept(rulesVisitor);
            if (throwOnError && rulesVisitor.Errors.Any(f => f.Level == MessageLevel.Error))
            {
                throw new IncorrectCronExpressionException(rulesVisitor.Errors.ToArray());
            }
            if(rulesVisitor.IsValid)
            {
                var timelineVisitor = new CronTimelineVisitor();
                ast.Accept(timelineVisitor);

                var evaluator = timelineVisitor.Evaluator;

                evaluator.ReferenceTime = request.ReferenceTime;
                evaluator = new TimeZoneCronForwardFireTimeEvaluatorDecorator(request.TargetTimeZoneInfo, evaluator);

                return new ConvertionResponse<ICronFireTimeEvaluator>(evaluator, rulesVisitor.Errors.ToArray());
            }
            return new ConvertionResponse<ICronFireTimeEvaluator>(null, rulesVisitor.Errors.ToArray());
        }

        public ConvertionResponse<ICronFireTimeEvaluator> Convert(CreateEvaluatorRequest request)
        {
            if (!request.Options.ProduceEndOfFileNode)
            {
                throw new ArgumentException("Produce end of file node option must be turned on to evaluate expression");
            }
            return base.Convert(request, (ast) => this.Convert(ast, request));
        }

        protected override ConvertionResponse<ICronFireTimeEvaluator> GetErrorResponse(Exception exc) 
            => new ConvertionResponse<ICronFireTimeEvaluator>(null, new FatalError(exc));
    }
}
