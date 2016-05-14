using System;
using Cron.Extensions.TimelineEvaluator.Evaluators;

namespace Cron.Converter
{
    class TimeZoneCronForwardFireTimeEvaluatorDecorator : ICronFireTimeEvaluator
    {
        private readonly TimeZoneInfo destinationZoneInfo;
        private readonly ICronFireTimeEvaluator evaluator;

        public TimeZoneCronForwardFireTimeEvaluatorDecorator(TimeZoneInfo destinationZoneInfo, ICronFireTimeEvaluator evaluator)
        {
            this.destinationZoneInfo = destinationZoneInfo;
            this.evaluator = evaluator;
        }

        public DateTimeOffset ReferenceTime
        {
            set
            {
                evaluator.ReferenceTime = value;
            }
        }

        public bool IsSatisfiedBy(DateTimeOffset time) => evaluator.IsSatisfiedBy(time);

        public DateTimeOffset? NextFire()
        {
            var generatedTime = evaluator.NextFire();

            if(generatedTime.HasValue)
                return TimeZoneInfo.ConvertTime(generatedTime.Value, destinationZoneInfo);
            return generatedTime;
        }
    }
}
