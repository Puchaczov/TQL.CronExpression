using System;
using System.Linq;
using TQL.Common.Evaluators;
using TQL.CronExpression.Exceptions;
using TQL.CronExpression.Parser.Nodes;
using TQL.CronExpression.TimelineEvaluator;
using TQL.CronExpression.Visitors;
using TQL.Interfaces;

namespace TQL.CronExpression
{
    public class CronTimeline : AbstractConverter<IFireTimeEvaluator>, IConverter<CreateEvaluatorRequest, ConvertionResponse<IFireTimeEvaluator>>
    {
        public CronTimeline(bool throwOnError = false)
            : base(throwOnError)
        { }

        private ConvertionResponse<IFireTimeEvaluator> Convert(RootComponentNode ast, CreateEvaluatorRequest request)
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

                return new ConvertionResponse<IFireTimeEvaluator>(new TimeZoneChangerDecorator(TimeZoneInfo.Local, request.TargetTimeZoneInfo, evaluator), rulesVisitor.Errors.ToArray());
            }
            return new ConvertionResponse<IFireTimeEvaluator>(null, rulesVisitor.Errors.ToArray());
        }

        public ConvertionResponse<IFireTimeEvaluator> Convert(CreateEvaluatorRequest request)
        {
            if (!request.Options.ProduceEndOfFileNode)
            {
                throw new ArgumentException("Produce end of file node option must be turned on to evaluate expression");
            }
            return base.Convert(request, (ast) => this.Convert(ast, request));
        }

        protected override ConvertionResponse<IFireTimeEvaluator> GetErrorResponse(Exception exc) 
            => new ConvertionResponse<IFireTimeEvaluator>(null, new FatalError(exc));
    }
}
