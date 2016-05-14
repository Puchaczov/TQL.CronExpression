using Cron.Exceptions;
using Cron.Parser.Nodes;
using Cron.Visitors;
using System.Linq;
using Cron.Converter;

namespace Cron.Converter
{
    public class CronValidator : AbstractConverter<bool>, IConverter<ConvertionRequest, ConvertionResponse<bool>>
    {
        public CronValidator(bool throwOnError = false)
            : base(throwOnError)
        { }

        public ConvertionResponse<bool> Convert(ConvertionRequest request) => base.Convert(request, Convert);

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
