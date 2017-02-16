using System;
using System.Linq;
using TQL.CronExpression.Exceptions;
using TQL.CronExpression.Parser.Nodes;
using TQL.CronExpression.Visitors;

namespace TQL.CronExpression
{
    public class CronValidator : AbstractConverter<bool>, IConverter<ConvertionRequest, ConvertionResponse<bool>>
    {
        public CronValidator(bool throwOnError = false)
            : base(throwOnError)
        { }

        public ConvertionResponse<bool> Convert(ConvertionRequest request) => base.Convert(request, Convert);

        protected override ConvertionResponse<bool> GetErrorResponse(Exception exc)
            => new ConvertionResponse<bool>(false, new FatalError(exc));

        private ConvertionResponse<bool> Convert(RootComponentNode ast)
        {
            var visitor = new CronRulesNodeVisitor(true);
            ast.Accept(visitor);
            if (throwOnError && visitor.Errors.Any(f => f.Level == MessageLevel.Error))
            {
                throw new IncorrectCronExpressionException(visitor.Errors.ToArray());
            }
            return new ConvertionResponse<bool>(visitor.IsValid, visitor.Errors.ToArray());
        }
    }
}
