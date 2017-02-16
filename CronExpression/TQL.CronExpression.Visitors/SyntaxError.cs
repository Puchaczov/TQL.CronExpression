using System.Collections.Generic;
using TQL.Core.Tokens;
using TQL.CronExpression.Parser.Enums;

namespace TQL.CronExpression.Visitors
{
    public class SyntaxError : VisitationMessage
    {
        private static readonly Dictionary<SyntaxErrorKind, Codes> Codes;

        static SyntaxError()
        {
            Codes = new Dictionary<SyntaxErrorKind, Codes>();
            Codes.Add(SyntaxErrorKind.MissingValue, Visitors.Codes.C02);
        }

        public SyntaxError(TextSpan[] spans, Segment segment, string message, SyntaxErrorKind kind)
            : base(spans, segment, message)
        {
            Kind = kind;
        }

        public SyntaxError(TextSpan span, Segment segment, string message, SyntaxErrorKind kind)
            : this(new[] {span}, segment, message, kind)
        {
        }

        public override MessageLevel Level => MessageLevel.Error;
        public SyntaxErrorKind Kind { get; }

        public override Codes Code => Codes[Kind];

        public override string ToString() => message;
    }
}