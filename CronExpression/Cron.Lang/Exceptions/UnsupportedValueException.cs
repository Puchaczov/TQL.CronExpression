using Cron.Parser.Tokens;
using System;
using System.Runtime.Serialization;

namespace Cron.Parser.Exceptions
{
    [Serializable]
    public class UnsupportedValueException : BaseCronValidationException
    {
        public UnsupportedValueException(Token token)
            : base(token)
        { }
    }
}