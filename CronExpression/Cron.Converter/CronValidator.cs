using Cron.Exceptions;
using Cron.Parser.Nodes;
using Cron.Visitors;
using System.Collections.Generic;
using System.Linq;
using System;

namespace Cron
{
    public class CronValidator : AbstractConverter<bool>
    {
        public CronValidator(bool throwOnError = false)
            : base(throwOnError)
        { }

        private static ConvertionResponse<bool> Convert(RootComponentNode ast)
        {
            var visitor = new CronRulesNodeVisitor(true);
            ast.Accept(visitor);
            if (visitor.Errors.Any(f => f.Level == MessageLevel.Error))
            {
                throw new IncorrectCronExpressionException(visitor.Errors.ToArray());
            }
            return new ConvertionResponse<bool>(visitor.IsValid, visitor.Errors.ToArray());
        }

        public override ConvertionResponse<bool> Convert(ConvertionRequest request) => base.Convert(request, Convert);
    }
}
