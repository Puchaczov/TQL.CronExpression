using System;

namespace Cron.Extensions.TimelineEvaluator.Evaluators
{
    public interface ICronFireTimeEvaluator
    {
        DateTimeOffset ReferenceTime { set; }
        DateTimeOffset? NextFire();
        DateTimeOffset? PreviousFire();
        bool IsSatisfiedBy(DateTimeOffset time);
    }
}
