using System;
using TQL.CronExpression.Parser.Enums;

namespace TQL.CronExpression.Visitors
{
    public class FatalVisitError : VisitationMessage
    {
        private readonly Exception _exc;

        public FatalVisitError(Exception exc)
            : base(null, Segment.Unknown, exc.Message)
        {
            this._exc = exc;
        }

        public override Codes Code => Codes.C01;
        public override MessageLevel Level => MessageLevel.Error;

        public override string ToString() => message;
    }
}