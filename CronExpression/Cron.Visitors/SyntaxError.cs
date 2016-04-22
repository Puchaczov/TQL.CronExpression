using Cron.Parser.Enums;
using Cron.Parser.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

    public abstract class Error
    {
        protected readonly Segment segment;
        protected readonly string message;
        protected readonly TextSpan[] spans;

        public abstract MessageLevel Level { get; }

        protected Error(TextSpan[] spans, Segment segment, string message)
        {
            this.segment = segment;
            this.message = message;
            this.spans = spans;
        }
    }

    public class SyntaxError : Error
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

    public class SemanticError : Error
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
}
