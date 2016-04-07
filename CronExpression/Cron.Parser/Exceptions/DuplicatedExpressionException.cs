using System;
using System.Runtime.Serialization;
using Cron.Parser.Tokens;

namespace Cron.Parser.Exceptions
{
    [Serializable]
    public class DuplicatedExpressionException : Exception
    {
        private readonly Token currentToken;
        private readonly int position;

        public DuplicatedExpressionException()
        {
        }

        public DuplicatedExpressionException(string message) : base(message)
        {
        }

        public DuplicatedExpressionException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public DuplicatedExpressionException(int position, Token currentToken)
        {
            this.position = position;
            this.currentToken = currentToken;
        }

        protected DuplicatedExpressionException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}