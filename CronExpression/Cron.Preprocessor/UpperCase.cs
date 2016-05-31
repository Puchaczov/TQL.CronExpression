using TQL.Common.Pipeline;

namespace TQL.CronExpression
{
    public class UpperCase : FilterBase<string>
    {
        protected override string Process(string input) => input.ToUpperInvariant();
    }
}
