using TQL.Core.Syntax;
using TQL.Core.Tokens;
using System;
using TQL.CronExpression.Parser.Tokens;
using TQL.CronExpression.Parser.Exceptions;

namespace TQL.CronExpression.Parser
{
    public class Lexer : LexerBase<Token>
    {
        public Lexer(string input)
            : base(input, new NoneToken(new TextSpan(0, 0)))
        { }

        public Token Last => (Token)lastToken.Clone();

        public override Token NextToken()
        {
            if (pos > input.Length - 1)
            {
                AssignTokenOfType(() => new EndOfFileToken(new TextSpan(input.Length, 0)));
                return currentToken;
            }

            var currentChar = input[pos];

            if (IsEndLine(currentChar))
            {
                var token = new WhiteSpaceToken(new TextSpan(pos, 2));
                pos += 2;
                return token;
            }

            if (IsDigit(currentChar))
            {
                return ConsumeInterger();
            }

            if (IsLetter(currentChar))
            {
                var tmpStartPos = Position;
                var letters = ConsumeLetters();
                var tmpStopPos = Position;
                switch (letters.Value)
                {
                    case "W":
                        return AssignTokenOfType(() => new WToken(new TextSpan(tmpStartPos, tmpStopPos - tmpStartPos)));
                    case "L":
                        return AssignTokenOfType(() => new LToken(new TextSpan(tmpStartPos, tmpStopPos - tmpStartPos)));
                    case "LW":
                        return AssignTokenOfType(() => new LWToken(new TextSpan(tmpStartPos, tmpStopPos - tmpStartPos)));
                    default:
                        return letters;
                }
            }

            if (IsMissing(currentChar))
            {
                return AssignTokenOfType(() => new MissingToken(new TextSpan(Position, 0)));
            }

            var lastPos = pos;
            pos += 1;
            switch (currentChar)
            {
                case '#':
                    return AssignTokenOfType(() => new HashToken(new TextSpan(lastPos, 1)));
                case ' ':
                    return AssignTokenOfType(() => new WhiteSpaceToken(new TextSpan(lastPos, 1)));
                case '?':
                    return AssignTokenOfType(() => new QuestionMarkToken(new TextSpan(lastPos, 1)));
                case ',':
                    return AssignTokenOfType(() => new CommaToken(new TextSpan(lastPos, 1)));
                case '*':
                    return AssignTokenOfType(() => new StarToken(new TextSpan(lastPos, 1)));
                case '-':
                    return AssignTokenOfType(() => new RangeToken(new TextSpan(lastPos, 1)));
                case '/':
                    return AssignTokenOfType(() => new IncrementByToken(new TextSpan(lastPos, 1)));
            }

            throw new UnknownTokenException(pos, currentChar);
        }

        private static bool IsMissing(char currentChar) => currentChar == '_';

        private Token ConsumeInterger()
        {
            var startPos = pos;
            var cnt = input.Length;
            while (cnt > pos && IsDigit(input[pos]))
            {
                ++pos;
            }

            return AssignTokenOfType(() => new IntegerToken(input.Substring(startPos, pos - startPos), new TextSpan(startPos, pos - startPos)));
        }

        private NameToken ConsumeLetters()
        {
            var startPos = pos;
            var cnt = input.Length;
            while (cnt > pos && IsLetter(input[pos]))
            {
                ++pos;
            }

            return AssignTokenOfType(() => new NameToken(input.Substring(startPos, pos - startPos), new TextSpan(startPos, pos - startPos))) as NameToken;
        }

        public override Token LastToken()
        {
            throw new NotImplementedException();
        }

        public override Token CurrentToken()
        {
            throw new NotImplementedException();
        }
    }
}
