using Cron.Visitors;
using System;

namespace Cron
{
    public class FatalError : CompilationMessage
    {
        private readonly Exception exc;

        public FatalError(Exception exc)
            : base(null, Parser.Enums.Segment.Unknown, string.Empty)
        {
            this.exc = exc;
        }

        public override MessageLevel Level => MessageLevel.Error;

        public override string ToString() => exc.Message;
    }
}