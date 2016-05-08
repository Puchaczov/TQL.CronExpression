using Cron.Utils.Filters;

namespace Cron
{
    public class UpperCase : FilterBase<string>
    {
        protected override string Process(string input) => input.ToUpperInvariant();
    }
}
