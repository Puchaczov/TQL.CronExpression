using Cron.Compilation;
using Cron.Compiler.Exceptions;
using Cron.Parser.Nodes;
using Cron.Visitors;
using System.Linq;

namespace Cron.Compiler
{
    public class CronRulesCheckCompiler : AbstractCompiler
    {
        public CronRulesCheckCompiler(bool throwOnError = false)
            : base(throwOnError)
        { }

        public CompilationResponse<bool> Compile(CompilationRequest request) => base.Compile(request, Convert);

        private static CompilationResponse<bool> Convert(RootComponentNode ast)
        {
            var visitor = new CronRulesNodeVisitor(true);
            ast.Accept(visitor);
            if(visitor.Errors.Any(f => f.Level == MessageLevel.Error))
            {
                throw new IncorrectCronExpressionException(visitor.Errors.ToArray());
            }
            return new CompilationResponse<bool>(visitor.IsValid, visitor.Errors.ToArray());
        }
    }
}
