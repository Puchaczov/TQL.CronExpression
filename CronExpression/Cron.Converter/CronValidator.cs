using Cron.Exceptions;
using Cron.Parser.Nodes;
using Cron.Visitors;
using System.Collections.Generic;
using System.Linq;

namespace Cron
{
    public class CronValidator : AbstractConverter
    {
        private bool isValid;
        private IEnumerable<VisitationMessage> messages;

        public CronValidator(bool throwOnError = false)
            : base(throwOnError)
        { }

        public bool IsValid() => isValid;

        public void Validate(string input, ConvertionRequest.CronMode mode)
        {
            var response = Convert(new ConvertionRequest(input, mode));
            isValid = response.Output;
            messages = response.Messages;
        }

        private ConvertionResponse<bool> Convert(ConvertionRequest request) => base.Convert(request, Convert);

        private static ConvertionResponse<bool> Convert(RootComponentNode ast)
        {
            var visitor = new CronRulesNodeVisitor(true);
            ast.Accept(visitor);
            if(visitor.Errors.Any(f => f.Level == MessageLevel.Error))
            {
                throw new IncorrectCronExpressionException(visitor.Errors.ToArray());
            }
            return new ConvertionResponse<bool>(visitor.IsValid, visitor.Errors.ToArray());
        }
    }
}
