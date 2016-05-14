using Cron.Common.Pipeline;

namespace Cron.Filters
{
    public class LowerCase : FilterBase<string>
    {
        protected override string Process(string input) => input.ToLowerInvariant();
    }
}
