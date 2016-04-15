using System;
using System.Runtime.Serialization;
using Cron.Parser.Tokens;

namespace Cron.Parser.Exceptions
{
    [Serializable]
    public class NestedExpressionException : Exception
    {
        private readonly int position;
        private readonly Token token;

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
                return $"There is to deep founded in position {position} when parsing token {token.Value}";
            }
        }

        public int Position
        {
            get
            {
                return position;
            }
        }
    }
}