using Cron.Exceptions;
using Cron.Extensions.TimelineEvaluator;
using Cron.Parser;
using Cron.Parser.Nodes;
using Cron.Utils;
using System;

namespace Cron
{
    public abstract class AbstractConverter
    {
        protected readonly bool throwOnError;

        protected AbstractConverter(bool throwOnError)
        {
            this.throwOnError = throwOnError;
        }

        protected virtual ConvertionResponse<T> Convert<T>(
            ConvertionRequest request,
            Func<RootComponentNode, ConvertionResponse<T>> fun)
            => Convert<T>(request, new ConvertionByFunc<RootComponentNode, ConvertionResponse<T>>(fun));

        protected virtual ConvertionResponse<T> Convert<T>(ConvertionRequest request, IConvertible<RootComponentNode, ConvertionResponse<T>> converter)
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
                return new ConvertionResponse<T>(new FatalError(exc));
            }
        }

        protected ConvertionResponse<IEvaluable<T>> Convert<T>(ConvertionRequest request, IConvertible<RootComponentNode, ConvertionResponse<IEvaluable<T>>> converter) => Convert(request, (ast) => converter.Convert(ast));

        protected virtual bool IsRequestValid(ConvertionRequest request) => request.Input != null && request.Options != null;
    }
}
