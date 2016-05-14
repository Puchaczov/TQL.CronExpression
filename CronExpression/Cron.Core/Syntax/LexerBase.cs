using Cron.Core.Tokens;
using System;
using System.Collections.Generic;

namespace Cron.Core.Syntax
{
    public abstract class LexerBase<TTokenType>: ILexer<TTokenType> where TTokenType: struct, IComparable, IFormattable
    {
        protected GenericToken<TTokenType> currentToken;
        protected GenericToken<TTokenType> lastToken;

        protected readonly Dictionary<char, char> endLines = new Dictionary<char, char>();

        protected readonly string input;
        protected int pos;

        protected LexerBase(string input, GenericToken<TTokenType> defaultToken)
        {
            if (input == null || input == string.Empty)
            {
                throw new ArgumentException(nameof(input));
            }
            this.input = input.Trim();

            pos = 0;
            currentToken = defaultToken;
            endLines.Add('\r', '\n');
        }

        public abstract GenericToken<TTokenType> NextToken();

        protected GenericToken<TTokenType> AssignTokenOfType(Func<GenericToken<TTokenType>> instantiate)
        {
            if (instantiate == null)
            {
                throw new ArgumentNullException(nameof(instantiate));
            }

            lastToken = currentToken;
            currentToken = instantiate();
            return currentToken;
        }
    }
}
