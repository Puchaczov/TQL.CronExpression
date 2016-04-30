using Cron.Parser.Enums;
using Cron.Parser.Tokens;
using System;

namespace Cron.Visitors
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

        public abstract MessageLevel Level { get; }
    }

    public class SyntaxError : VisitationMessage
    {
        private readonly SyntaxErrorKind kind;

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

        public override string ToString() => message;
    }

    public class SemanticError : VisitationMessage
    {
        private readonly SemanticErrorKind kind;

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
    }

    public class FatalVisitError : VisitationMessage
    {
        private readonly Exception exc;

        public FatalVisitError(Exception exc)
            : base(null, Segment.Unknown, string.Empty)
        {
            this.exc = exc;
        }

        public override MessageLevel Level => MessageLevel.Error;
    }
}
