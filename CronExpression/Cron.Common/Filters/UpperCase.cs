using Cron.Common.Pipeline;

namespace Cron.Common.Filters
{
    public class UpperCase : FilterBase<string>
    {
        protected override string Process(string input) => input.ToUpperInvariant();
    }
}
