using System;

namespace TQL.CronExpression
{
    public class CreateEvaluatorRequest : ConvertionRequest
    {
        public CreateEvaluatorRequest(string input, CronMode mode, DateTime referenceTime,
            TimeZoneInfo source, TimeZoneInfo destination)
            : base(input, mode)
        {
            ReferenceTime = referenceTime;
            Source = source;
            Destination = destination;
        }

        public CreateEvaluatorRequest(string input, ConvertionOptions options, DateTime referenceTime,
            TimeZoneInfo source, TimeZoneInfo destination)
            : base(input, options)
        {
            ReferenceTime = referenceTime;
            Source = source;
            Destination = destination;
        }

        public DateTime ReferenceTime { get; }

        public TimeZoneInfo Source { get; }
        public TimeZoneInfo Destination { get; }
    }
}