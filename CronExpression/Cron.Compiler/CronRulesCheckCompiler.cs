using Cron.Compilation;
using Cron.Parser.Nodes;
using Cron.Visitors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cron.Compiler
{
    public class CronRulesCheckCompiler : AbstractCompiler
    {
        public CompilationResponse<bool> Compile(CompilationRequest request) => base.Compile<bool>(request, Convert);

        private static CompilationResponse<bool> Convert(RootComponentNode ast)
        {
            var visitor = new CronRulesNodeVisitor(true);
            ast.Accept(visitor);
            return new CompilationResponse<bool>(visitor.IsValid, visitor.Errors.ToArray());
        }
    }
}
