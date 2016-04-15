using Cron.Compilation;
using Cron.Parser;
using Cron.Parser.Nodes;
using Cron.Utils;
using Cron.Visitors;
using System;

namespace Cron
{
    public abstract class AbstractCompiler
    {
        public virtual T Compile<T>(CompilationRequest request, Func<RootComponentNode, T> fun) => Compile<T>(request, new ConvertionByFunc<RootComponentNode, T>(fun));

        public virtual T Compile<T>(CompilationRequest request, IConvertible<RootComponentNode, T> converter)
        {
            if (!IsRequestValid(request))
            {
                throw new ArgumentException();
            }
            var preprocessor = new Preprocessor();
            var input = preprocessor.Execute(request.Input);
            var lexer = new Lexer(input);
            var parser = new CronParser(lexer, request.Options.ProduceYearIfMissing, request.Options.ProduceEndOfFileNode);
            return converter.Convert(parser.ComposeRootComponents());
        }

        public IEvaluable<T> Compile<T>(CompilationRequest request, IConvertible<RootComponentNode, IEvaluable<T>> converter) => Compile(request, (ast) => converter.Convert(ast));

        protected virtual bool IsRequestValid(CompilationRequest request) => request.Input != null && request.Options != null;
    }
}
