using System.Collections.Generic;
using TQL.Core.Tokens;
using TQL.CronExpression.Parser.Enums;

namespace TQL.CronExpression.Visitors
{
    public class SemanticError : VisitationMessage
    {
        private static readonly Dictionary<SemanticErrorKind, Codes> Codes;

        static SemanticError()
        {
            Codes = new Dictionary<SemanticErrorKind, Codes>();
            Codes.Add(SemanticErrorKind.CountMismatched, Visitors.Codes.C03);
            Codes.Add(SemanticErrorKind.SwappedValue, Visitors.Codes.C04);
            Codes.Add(SemanticErrorKind.UnsupportedValue, Visitors.Codes.C05);
            Codes.Add(SemanticErrorKind.ValueOutOfRange, Visitors.Codes.C06);
        }

        public SemanticError(TextSpan span, Segment segment, string message, SemanticErrorKind kind)
            : base(new[] {span}, segment, message)
        {
            Kind = kind;
        }

        public SemanticError(TextSpan[] spans, Segment segment, string message, SemanticErrorKind kind)
            : base(spans, segment, message)
        {
            Kind = kind;
        }

        public override MessageLevel Level => MessageLevel.Error;
        public SemanticErrorKind Kind { get; }

        public override Codes Code => Codes[Kind];
        public override string ToString() => message;
    }
}