using System;

namespace Cron.Extensions.TimelineEvaluator.Evaluators
{
    public interface ICronFireTimeEvaluator
    {
        DateTime ReferenceTime { set; }
        DateTimeOffset OffsetReferenceTime { set; }
        DateTime? NextFire();
        DateTime? PreviousFire();
        bool IsSatisfiedBy(DateTime time);
    }
}
