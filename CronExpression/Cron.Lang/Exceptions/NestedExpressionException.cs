using System;
using System.Runtime.Serialization;
using Cron.Parser.Tokens;

namespace Cron.Parser.Exceptions
{
    [Serializable]
    internal class NestedExpressionException : Exception
    {
        private int position;
        private Token token;

        public int Position
        {
            get
            {
                return position;
            }
        }

        public NestedExpressionException()
        {
        }

        public NestedExpressionException(string message) : base(message)
        {
        }

        public NestedExpressionException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public NestedExpressionException(int position, Token token)
        {
            this.position = position;
            this.token = token;
        }

        protected NestedExpressionException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public override string Message
        {
            get
            {
                return string.Format("There is to deep founded in position {0} when parsing token {1}", position, token.Value);
            }
        }
    }
}