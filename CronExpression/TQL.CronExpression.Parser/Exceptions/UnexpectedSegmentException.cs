using System;
using TQL.CronExpression.Parser.Enums;

namespace TQL.CronExpression.Parser.Exceptions
{
    public class UnexpectedSegmentException : Exception
    {
        private readonly Segment _segment;

        public UnexpectedSegmentException(Segment segment)
        {
            this._segment = segment;
        }
    }
}