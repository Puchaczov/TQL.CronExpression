using Cron.Parser.Tokens;
using System;

namespace Cron.Visitors.Exceptions
{
    public class BaseCronValidationException : Exception
    {
        public Token Token { get; set; }

        public BaseCronValidationException(Token token)
        {
            this.Token = token;
        }
    }
}