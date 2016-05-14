using Cron.Interfaces;
using System;

namespace Cron.Extensions.TimelineEvaluator.Evaluators
{
    public interface ICronFireTimeEvaluator : IFireTimeEvaluator
    {
        DateTimeOffset ReferenceTime { set; }
        DateTimeOffset? PreviousFire();
        bool IsSatisfiedBy(DateTimeOffset time);
    }
}
