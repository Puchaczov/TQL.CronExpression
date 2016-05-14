using Cron.Common.Pipeline;

namespace Cron.Filters
{
    public class Trim : FilterBase<string>
    {
        protected override string Process(string input) => input.Trim();
    }
}
