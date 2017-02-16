using System;

namespace TQL.CronExpression
{
    public class CreateEvaluatorRequest : ConvertionRequest
    {
        public CreateEvaluatorRequest(string input, CronMode mode, DateTimeOffset referenceTime,
            TimeZoneInfo targetTimeZoneInfo)
            : base(input, mode)
        {
            ReferenceTime = referenceTime;
            TargetTimeZoneInfo = targetTimeZoneInfo;
        }

        public CreateEvaluatorRequest(string input, ConvertionOptions options, DateTimeOffset referenceTime,
            TimeZoneInfo targetTimeZoneInfo)
            : base(input, options)
        {
            ReferenceTime = referenceTime;
            TargetTimeZoneInfo = targetTimeZoneInfo;
        }

        public DateTimeOffset ReferenceTime { get; }

        public TimeZoneInfo TargetTimeZoneInfo { get; }
    }
}