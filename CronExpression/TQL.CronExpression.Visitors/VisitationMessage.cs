using System.Collections.Generic;
using TQL.Core.Tokens;
using TQL.CronExpression.Parser.Enums;

namespace TQL.CronExpression.Visitors
{
    public abstract class VisitationMessage
    {
        protected readonly string message;
        protected readonly Segment segment;
        protected readonly TextSpan[] spans;

        protected VisitationMessage(TextSpan[] spans, Segment segment, string message)
        {
            this.segment = segment;
            this.message = message;
            this.spans = spans;
        }

        public string Message => message;
        public Segment Segment => segment;

        public abstract MessageLevel Level { get; }
        public abstract Codes Code { get; }
        public IReadOnlyCollection<TextSpan> Spans => spans;
        public abstract override string ToString();
    }
}