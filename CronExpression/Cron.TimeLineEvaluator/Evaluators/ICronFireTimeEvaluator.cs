using System;
using TQL.Interfaces;

namespace Cron.Extensions.TimelineEvaluator.Evaluators
{
    public interface ICronFireTimeEvaluator : IFireTimeEvaluator
    {
        DateTimeOffset ReferenceTime { set; }
        bool IsSatisfiedBy(DateTimeOffset time);
    }
}
