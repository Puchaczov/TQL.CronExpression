using System;
using Cron.Parser.Tokens;

namespace Cron.Parser.Exceptions
{
    public class DuplicatedExpressionException : Exception
    {
        private Token currentToken;
        private int position;

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
    }
}