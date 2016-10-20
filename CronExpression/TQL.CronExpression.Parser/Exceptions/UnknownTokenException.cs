﻿using System;

namespace TQL.CronExpression.Parser.Exceptions
{
    public class UnknownTokenException : Exception
    {
        private readonly char currentChar;
        private readonly int pos;

        public UnknownTokenException(int pos, char currentChar, string message)
            : base(message)
        {
            this.pos = pos;
            this.currentChar = currentChar;
        }
    }
}