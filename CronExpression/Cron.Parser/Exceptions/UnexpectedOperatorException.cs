using System;
using System.Runtime.Serialization;
using Cron.Parser.Tokens;

namespace Cron.Parser
{
    [Serializable]
    public class UnexpectedOperatorException : Exception
    {
        private Token currentToken;
        private int position;

        public UnexpectedOperatorException()
        {
        }

        public UnexpectedOperatorException(string message) : base(message)
        {
        }

        public UnexpectedOperatorException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public UnexpectedOperatorException(int position, Token currentToken)
        {
            this.position = position;
            this.currentToken = currentToken;
        }

        protected UnexpectedOperatorException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}