using Cron.Parser.Enums;
using Cron.Parser.Exceptions;
using Cron.Parser.Tokens;
using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace Cron.Parser
{
    public class Lexer
    {
        private string input;
        private int pos;
        private Token currentToken;

        public int Position
        {
            get
            {
                return pos;
            }
        }

        public char Character
        {
            get
            {
                return input[pos];
            }
        }

        public Lexer(string input)
        {
            this.input = input.Trim();
            this.pos = 0;
            this.currentToken = new NoneToken();
        }

        public Token NextToken()
        {
            if(pos > input.Count() - 1)
            {
                return new EndOfFileToken();
            }

            char currentChar = input[pos];

            if(IsDigit(currentChar))
            {
                return ConsumeInterger();
            }

            if(IsLetter(currentChar))
            {
                var letters = ConsumeLetters();
                switch(letters.Value)
                {
                    case "W":
                        return new WToken();
                    case "L":
                        return new LToken();
                    case "LW":
                        return new LWToken();
                    default:
                        return letters;
                }
            }

            pos += 1;
            switch(currentChar)
            {
                case '#':
                    return new HashToken();
                case ' ':
                    return new WhiteSpaceToken();
                case '?':
                    return new QuestionMarkToken();
                case ',':
                    return new CommaToken();
                case '*':
                    return new StarToken();
                case '-':
                    return new RangeToken();
                case '/':
                    return new IncrementByToken();
            }

            throw new UnknownTokenException(pos, currentChar);
        }

        private NameToken ConsumeLetters()
        {
            int startPos = pos;
            int cnt = input.Count();
            while (cnt > pos && IsLetter(input[pos]))
            {
                ++pos;
            }
            return new NameToken(input.Substring(startPos, pos - startPos));
        }

        private bool IsLetter(char currentChar)
        {
            if(Regex.IsMatch(currentChar.ToString(), "[a-zA-Z]+"))
            {
                return true;
            }
            return false;
        }

        private Token ConsumeInterger()
        {
            int startPos = pos;
            int cnt = input.Count();
            while (cnt > pos && IsDigit(input[pos]))
            {
                ++pos;
            }
            return new IntegerToken(input.Substring(startPos, pos - startPos));
        }

        public bool IsDigit(char letter)
        {
            if(letter >= '0' && letter <= '9')
            {
                return true;
            }
            return false;
        }

        public bool IsNumeric(string value)
        {
            int result = 0;
            if(int.TryParse(value, out result))
            {
                return true;
            }
            return false;
        }
    }
}
