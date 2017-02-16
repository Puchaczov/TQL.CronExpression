using System;

namespace TQL.CronExpression.Parser.Exceptions
{
    public class UnknownSegmentException : Exception
    {
        private readonly int _v;

        public UnknownSegmentException(int v)
        {
            this._v = v;
        }
    }
}