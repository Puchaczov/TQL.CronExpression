using Cron.Filters;
using Cron.Utils.Filters;

namespace Cron
{
    public class Preprocessor : Pipeline<string>
    {
        public Preprocessor()
        {
            base
                .Register(new Trim())
                .Register(new LowerCase())
                .Register(new ReplaceNonStandardDefinitions());
        }
    }
}
