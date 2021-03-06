﻿using System;
using TQL.CronExpression.Parser.Tokens;

namespace TQL.CronExpression.Visitors.Exceptions
{
    public class BaseCronValidationException : Exception
    {
        public BaseCronValidationException(Token token)
        {
            Token = token;
        }

        public Token Token { get; set; }
    }
}