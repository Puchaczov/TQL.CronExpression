using TQL.Common.Filters.Pipeline;

namespace TQL.CronExpression.Preprocessor
{
    public class ReplaceNonStandardDefinitions : FilterBase<string>
    {
        protected override string Process(string input)
        {
            switch(input)
            {
                case "@YEARLY":
                case "@ANNUALLY":
                    return "0 0 0 1 1 * *";
                case "@MONTHLY":
                    return "0 0 0 1 * * *";
                case "@WEEKLY":
                    return "0 0 0 * * 0 *";
                case "@DAILY":
                    return "0 0 0 * * * *";
                case "@HOURLY":
                    return "0 0 * * * * *";
            }
            return input;
        }
    }
}
