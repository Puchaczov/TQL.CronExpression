using Cron.Parser.Tokens;
using System;

namespace Cron.Visitors.Exceptions
{
    public class UnexpectedTokenException : Exception
    {
        public int Position { get; private set; }
        public Token Token { get; private set; }

        public UnexpectedTokenException(int pos, Token token)
        {
            this.Position = pos;
        }

        public override string Message
        {
            get
            {
                return string.Format("Unexpected token {0} occured at position {1}", Token.Value, Position);
            }
        }
    }
}
