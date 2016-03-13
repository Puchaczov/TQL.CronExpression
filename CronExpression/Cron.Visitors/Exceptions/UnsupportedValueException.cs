using Cron.Parser.Tokens;
using System;

namespace Cron.Visitors.Exceptions
{
    public class UnsupportedValueException : BaseCronValidationException
    {
        public UnsupportedValueException(Token token)
            : base(token)
        { }
    }
}