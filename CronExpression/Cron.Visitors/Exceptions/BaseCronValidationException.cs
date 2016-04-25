using Cron.Parser.Tokens;
using System;

namespace Cron.Visitors.Exceptions
{
    public class BaseCronValidationException : Exception
    {
        public BaseCronValidationException(Token token)
        {
            this.Token = token;
        }

        public Token Token { get; set; }
    }
}