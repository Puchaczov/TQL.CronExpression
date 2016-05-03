using Cron.Visitors;
using System;

namespace Cron.Exceptions
{
    public class FatalError : VisitationMessage
    {
        private readonly Exception exc;

        public FatalError(Exception exc)
            : base(null, Parser.Enums.Segment.Unknown, string.Empty)
        {
            this.exc = exc;
        }

        public override Codes Code => Codes.C01;

        public override MessageLevel Level => MessageLevel.Error;

        public override string ToString() => exc.Message;
    }
}