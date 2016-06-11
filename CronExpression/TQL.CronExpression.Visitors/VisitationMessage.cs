using System;
using System.Collections.Generic;
using TQL.Core.Tokens;
using TQL.CronExpression.Parser.Enums;

namespace TQL.CronExpression.Visitors
{
    public enum SyntaxErrorKind
    {
        MissingValue
    }

    public enum SemanticErrorKind
    {
        SwappedValue,
        UnsupportedValue,
        ValueOutOfRange,
        CountMismatched,
    }

    public enum MessageLevel
    {
        Info,
        Warning,
        Error
    }

    public enum Codes : uint
    {
        C01,
        C02,
        C03,
        C04,
        C05,
        C06
    }

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

    public class SyntaxError : VisitationMessage
    {
        private readonly SyntaxErrorKind kind;

        private static readonly Dictionary<SyntaxErrorKind, Codes> codes;

        static SyntaxError()
        {
            codes = new Dictionary<SyntaxErrorKind, Codes>();
            codes.Add(SyntaxErrorKind.MissingValue, Codes.C02);
        }

        public SyntaxError(TextSpan[] spans, Segment segment, string message, SyntaxErrorKind kind)
            : base(spans, segment, message)
        {
            this.kind = kind;
        }

        public SyntaxError(TextSpan span, Segment segment, string message, SyntaxErrorKind kind)
            : this(new TextSpan[] { span }, segment, message, kind)
        { }

        public override MessageLevel Level => MessageLevel.Error;
        public SyntaxErrorKind Kind => kind;
        public override Codes Code => codes[kind];

        public override string ToString() => message;
    }

    public class SemanticError : VisitationMessage
    {
        private readonly SemanticErrorKind kind;
        private static readonly Dictionary<SemanticErrorKind, Codes> codes;

        static SemanticError()
        {
            codes = new Dictionary<SemanticErrorKind, Codes>();
            codes.Add(SemanticErrorKind.CountMismatched, Codes.C03);
            codes.Add(SemanticErrorKind.SwappedValue, Codes.C04);
            codes.Add(SemanticErrorKind.UnsupportedValue, Codes.C05);
            codes.Add(SemanticErrorKind.ValueOutOfRange, Codes.C06);
        }

        public SemanticError(TextSpan span, Segment segment, string message, SemanticErrorKind kind)
            : base(new TextSpan[] { span }, segment, message)
        {
            this.kind = kind;
        }

        public SemanticError(TextSpan[] spans, Segment segment, string message, SemanticErrorKind kind)
            : base(spans, segment, message)
        {
            this.kind = kind;
        }

        public override MessageLevel Level => MessageLevel.Error;
        public SemanticErrorKind Kind => kind;
        public override Codes Code => codes[kind];
        public override string ToString() => this.message;
    }

    public class FatalVisitError : VisitationMessage
    {
        private readonly Exception exc;

        public FatalVisitError(Exception exc)
            : base(null, Segment.Unknown, exc.Message)
        {
            this.exc = exc;
        }

        public override Codes Code => Codes.C01;
        public override MessageLevel Level => MessageLevel.Error;

        public override string ToString() => this.message;
    }
}
