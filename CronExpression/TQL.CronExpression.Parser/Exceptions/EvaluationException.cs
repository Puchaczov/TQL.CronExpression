using System;
using TQL.CronExpression.Parser.Enums;

namespace TQL.CronExpression.Parser.Exceptions
{
    public class EvaluationException : Exception
    {
        private readonly Segment segment;
        private readonly string message;

        public EvaluationException(Segment segment, string message)
        {
            this.segment = segment;
            this.message = message;
        }

        public Segment Segment => segment;

        public override string Message => this.message;
    }
}
