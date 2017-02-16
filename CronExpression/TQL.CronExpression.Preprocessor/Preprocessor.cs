using TQL.Common.Filters;
using TQL.Common.Filters.Pipeline;

namespace TQL.CronExpression.Preprocessor
{
    public class Preprocessor : Pipeline<string>
    {
        public Preprocessor()
        {
            base
                .Register(new ReplaceChar(char.ConvertFromUtf32(160)[0], ' '))
                .Register(new Trim())
                .Register(new UpperCase())
                .Register(new ReplaceNonStandardDefinitions());
        }
    }
}
