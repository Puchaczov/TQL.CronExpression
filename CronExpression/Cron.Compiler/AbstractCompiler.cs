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
        public virtual T Compile<T>(CompilationRequest request, Func<RootComponentNode, T> fun)
        {
            return Compile<T>(request, new ConvertionByFunc<RootComponentNode, T>(fun));
        }

        public virtual T Compile<T>(CompilationRequest request, IConvertible<RootComponentNode, T> converter)
        {
            if (!IsRequestValid(request))
            {
                throw new ArgumentException();
            }
            Preprocessor preprocessor = new Preprocessor();
            string input = preprocessor.Execute(request.Input);
            Lexer lexer = new Lexer(input);
            CronParser parser = new CronParser(lexer, request.Options.ProduceYearIfMissing, request.Options.ProduceEndOfFileNode);
            return converter.Convert(parser.ComposeRootComponents());
        }

        public IEvaluable<T> Compile<T>(CompilationRequest request, IConvertible<RootComponentNode, IEvaluable<T>> converter)
        {
            return Compile(request, (ast) => converter.Convert(ast));
        }

        protected virtual bool IsRequestValid(CompilationRequest request)
        {
            return request.Input != null && request.Options != null;
        }
    }
}
