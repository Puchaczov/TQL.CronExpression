using Cron.Compilation;
using Cron.Compiler;
using Cron.Compiler.Exceptions;
using Cron.Parser;
using Cron.Parser.Nodes;
using Cron.Utils;
using Cron.Visitors;
using System;

namespace Cron
{
    public abstract class AbstractCompiler
    {
        private readonly bool throwOnError;

        protected AbstractCompiler(bool throwOnError)
        {
            this.throwOnError = throwOnError;
        }

        protected virtual CompilationResponse<T> Compile<T>(
            CompilationRequest request,
            Func<RootComponentNode, CompilationResponse<T>> fun)
            => Compile<T>(request, new ConvertionByFunc<RootComponentNode, CompilationResponse<T>>(fun));

        protected virtual CompilationResponse<T> Compile<T>(CompilationRequest request, IConvertible<RootComponentNode, CompilationResponse<T>> converter)
        {
            try
            {
                if (!IsRequestValid(request))
                {
                    throw new ArgumentException();
                }
                var preprocessor = new Preprocessor();
                var input = preprocessor.Execute(request.Input);
                var lexer = new Lexer(input);
                var parser = new CronParser(lexer, request.Options.ProduceYearIfMissing, request.Options.ProduceEndOfFileNode, request.Options.ProduceSecondsIfMissing);
                var ast = parser.ComposeRootComponents();
                return converter.Convert(ast);
            }
            catch(IncorrectCronExpressionException exc)
            {
                if(throwOnError)
                {
                    throw;
                }
                return new CompilationResponse<T>(new FatalError(exc));
            }
        }

        protected CompilationResponse<IEvaluable<T>> Compile<T>(CompilationRequest request, IConvertible<RootComponentNode, CompilationResponse<IEvaluable<T>>> converter) => Compile(request, (ast) => converter.Convert(ast));

        protected virtual bool IsRequestValid(CompilationRequest request) => request.Input != null && request.Options != null;
    }
}
