using Cron.Parser.Enums;
using System;

namespace Cron.Parser.Exceptions
{
    public class EvaluationException : Exception
    {
        private readonly Segment segment;
        private string message;

        public EvaluationException(Segment segment, string message)
        {
            this.segment = segment;
            this.message = message;
        }

        public Segment Segment => segment;

        public override string Message => this.message;
    }
}
