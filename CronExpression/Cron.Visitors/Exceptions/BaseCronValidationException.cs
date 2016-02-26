using Cron.Parser.Tokens;
using System;
using System.Runtime.Serialization;

namespace Cron.Visitors.Exceptions
{
    [Serializable]
    public class BaseCronValidationException : Exception
    {
        public Token Token { get; set; }

        public BaseCronValidationException(Token token)
        {
            this.Token = token;
        }

        protected BaseCronValidationException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}