using System;
using TQL.Interfaces;

namespace TQL.CronExpression.TimelineEvaluator.Evaluators
{
    public interface ICronFireTimeEvaluator : IFireTimeEvaluator
    {
        DateTimeOffset ReferenceTime { set; }
    }
}