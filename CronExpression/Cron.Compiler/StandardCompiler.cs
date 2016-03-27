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
    public class StandardCompiler : AbstractCompiler
    {
        public ICronFireTimeEvaluator Compile(CompilationRequest request)
        {
            if(!request.Options.ProduceEndOfFileNode)
            {
                throw new ArgumentException("Produce end of file node option must be turned on to evaluate expression");
            }
            return base.Compile(request, Convert);
        }

        private ICronFireTimeEvaluator Convert(RootComponentNode input)
        {
            CronTimelineVisitor visitor = new CronTimelineVisitor();
            input.Accept(visitor);
            return visitor.Evaluator;
        }
    }
}
