using TQL.Common.Pipeline;

namespace TQL.CronExpression.Filters
{
    public class LowerCase : FilterBase<string>
    {
        protected override string Process(string input) => input.ToLowerInvariant();
    }
}
