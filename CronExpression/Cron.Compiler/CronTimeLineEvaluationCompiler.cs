using Cron.Compilation;
using Cron.Compiler.Exceptions;
using Cron.Extensions.TimelineEvaluator;
using Cron.Extensions.TimelineEvaluator.Evaluators;
using Cron.Parser.Nodes;
using Cron.Visitors;
using System;
using System.Linq;

namespace Cron.Compiler
{
    public class CronTimeLineEvaluationCompiler : AbstractCompiler
    {
        public CronTimeLineEvaluationCompiler(bool throwOnError = false)
            : base(throwOnError)
        { }

        public CompilationResponse<ICronFireTimeEvaluator> Compile(CompilationRequest request)
        {
            if(!request.Options.ProduceEndOfFileNode)
            {
                throw new ArgumentException("Produce end of file node option must be turned on to evaluate expression");
            }
            return base.Compile(request, Convert);
        }

        private static CompilationResponse<ICronFireTimeEvaluator> Convert(RootComponentNode ast)
        {
            var visitor = new CronTimelineVisitor();
            ast.Accept(visitor);
            if (visitor.Errors.Any(f => f.Level == MessageLevel.Error))
            {
                throw new IncorrectCronExpressionException(visitor.Errors.ToArray());
            }
            return new CompilationResponse<ICronFireTimeEvaluator>(visitor.Errors.Count() == 0 ? visitor.Evaluator : null, visitor.Errors.ToArray());
        }
    }
}
