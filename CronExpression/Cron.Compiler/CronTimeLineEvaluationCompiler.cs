using Cron.Compilation;
using Cron.Parser.Nodes;
using Cron.Utils;
using Cron.Visitors;
using Cron.Visitors.Evaluators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cron.Compiler
{
    public class CronTimeLineEvaluationCompiler : AbstractCompiler
    {
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
            return new CompilationResponse<ICronFireTimeEvaluator>(visitor.Errors.Count() == 0 ? visitor.Evaluator : null, visitor.Errors.ToArray());
        }
    }
}
