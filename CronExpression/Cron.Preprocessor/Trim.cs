using TQL.Common.Pipeline;

namespace TQL.CronExpression.Filters
{
    public class Trim : FilterBase<string>
    {
        protected override string Process(string input) => input.Trim();
    }
}
