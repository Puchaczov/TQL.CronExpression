﻿using System;
using TQL.CronExpression.Parser.Tokens;

namespace TQL.CronExpression.Parser.Exceptions
{
    public class UnexpectedTokenException : Exception
    {
        public UnexpectedTokenException(int pos, Token token)
        {
            this.Position = pos;
            this.Token = token;
        }

        public override string Message => $"Unexpected token {Token.Value} occured at position {Position}";
        public int Position { get; }
        public Token Token { get; }
    }
}
