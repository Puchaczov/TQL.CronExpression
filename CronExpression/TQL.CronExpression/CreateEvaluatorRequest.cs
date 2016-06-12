using System;

namespace TQL.CronExpression.Converter
{
    public class CreateEvaluatorRequest : ConvertionRequest
    {
        private readonly DateTimeOffset referenceTime;
        private readonly TimeZoneInfo targetTimeZoneInfo;

        public CreateEvaluatorRequest(string input, CronMode mode, DateTimeOffset referenceTime, TimeZoneInfo targetTimeZoneInfo)
            :base(input, mode)
        {
            this.referenceTime = referenceTime;
            this.targetTimeZoneInfo = targetTimeZoneInfo;
        }

        public CreateEvaluatorRequest(string input, ConvertionOptions options, DateTimeOffset referenceTime, TimeZoneInfo targetTimeZoneInfo)
            : base(input, options)
        {
            this.referenceTime = referenceTime;
            this.targetTimeZoneInfo = targetTimeZoneInfo;
        }

        public DateTimeOffset ReferenceTime => referenceTime;
        public TimeZoneInfo TargetTimeZoneInfo => targetTimeZoneInfo;
    }
}
