using System;
using System.Runtime.Serialization;
using Cron.Parser.Enums;
using Cron.Parser.Tokens;

namespace Cron.Visitors.Exceptions
{
    [Serializable]
    public class UnexpectedPrecededWNodeAtSegmentException : BaseCronValidationException
    {
        private readonly Segment segment;

        public UnexpectedPrecededWNodeAtSegmentException(Token token, Segment segment)
            : base(token)
        {
            this.segment = segment;
        }
    }
}