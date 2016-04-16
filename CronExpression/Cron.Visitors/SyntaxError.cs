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
        CountMismatched,
        ValueOutOfRange,
        UnsupportedValue,
        SwappedValue,
    }

    public class SyntaxError
    {
        private readonly Segment segment;
        private readonly string message;
        private readonly TextSpan[] spans;
        private readonly SyntaxErrorKind kind;

        public SyntaxErrorKind Kind
        {
            get
            {
                return kind;
            }
        }

        public SyntaxError(TextSpan[] spans, Segment segment, string message, SyntaxErrorKind kind)
        {
            this.segment = segment;
            this.message = message;
            this.spans = spans;
            this.kind = kind;
        }

        public SyntaxError(TextSpan span, Segment segment, string message, SyntaxErrorKind kind)
            : this(new TextSpan[] { span }, segment, message, kind)
        { }

        public override string ToString() => message;
    }
}
