using Cron.Parser.Tokens;
using System;
using System.Runtime.Serialization;

namespace Cron.Parser.Exceptions
{
    [Serializable]
    public class UnexpectedTokenException : Exception
    {

        public UnexpectedTokenException(int pos, Token token)
        {
            this.Position = pos;
            this.Token = token;
        }

        protected UnexpectedTokenException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public override string Message
        {
            get
            {
                return $"Unexpected token {Token.Value} occured at position {Position}";
            }
        }
        public int Position { get; }
        public Token Token { get; }
    }
}
