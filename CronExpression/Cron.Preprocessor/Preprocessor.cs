using Cron.Filters;
using TQL.Common.Pipeline;

namespace Cron
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
