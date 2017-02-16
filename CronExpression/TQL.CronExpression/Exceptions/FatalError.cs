using System;
using TQL.CronExpression.Visitors;

namespace TQL.CronExpression.Exceptions
{
    public class FatalError : VisitationMessage
    {
        private readonly Exception _exc;

        public FatalError(Exception exc)
            : base(null, Parser.Enums.Segment.Unknown, string.Empty)
        {
            this._exc = exc;
        }

        public override Codes Code => Codes.C01;

        public override MessageLevel Level => MessageLevel.Error;

        public override string ToString() => _exc.Message;
    }
}