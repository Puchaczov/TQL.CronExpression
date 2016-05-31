using System;
using TQL.Interfaces;

namespace TQL.CronExpression.Extensions.TimelineEvaluator.Evaluators
{
    public interface ICronFireTimeEvaluator : IFireTimeEvaluator
    {
        DateTimeOffset ReferenceTime { set; }
        bool IsSatisfiedBy(DateTimeOffset time);
    }
}
