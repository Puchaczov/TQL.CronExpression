using TQL.Core.Converters;
using TQL.CronExpression.Parser;
using TQL.CronExpression.Parser.Enums;
using TQL.CronExpression.Parser.Nodes;
using TQL.CronExpression.Parser.Visitors;

namespace TQL.CronExpression.Converter
{
    public abstract class AbstractConverter<TOutput> : ConverterBase<TOutput, ConvertionResponse<TOutput>, INodeVisitor, TokenType, RootComponentNode, ConvertionRequest>
    {
        protected readonly bool throwOnError;

        protected AbstractConverter(bool throwOnError)
        {
            this.throwOnError = throwOnError;
        }

        protected override RootComponentNode InstantiateRootNodeFromRequest(ConvertionRequest request)
        {
            var preprocessor = new Preprocessor();
            var input = preprocessor.Execute(request.Input);
            var lexer = new Lexer(input);
            var parser = new CronParser(lexer, request.Options.ProduceYearIfMissing, request.Options.ProduceEndOfFileNode, request.Options.ProduceSecondsIfMissing);
            return parser.ComposeRootComponents();
        }

        protected override bool IsValid(ConvertionRequest request) => request.Input != null && request.Options != null;
    }
}
