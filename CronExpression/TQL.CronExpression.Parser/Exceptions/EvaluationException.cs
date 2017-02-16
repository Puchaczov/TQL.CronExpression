using System;
using TQL.CronExpression.Parser.Enums;

namespace TQL.CronExpression.Parser.Exceptions
{
    public class EvaluationException : Exception
    {
        public EvaluationException(Segment segment, string message)
        {
            Segment = segment;
            Message = message;
        }

        public Segment Segment { get; }

        public override string Message { get; }
    }
}