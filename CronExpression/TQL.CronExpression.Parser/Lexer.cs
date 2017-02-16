using TQL.Core.Syntax;
using TQL.Core.Tokens;
using System;
using TQL.CronExpression.Parser.Tokens;
using TQL.CronExpression.Parser.Enums;
using System.Text.RegularExpressions;
using System.Linq;

namespace TQL.CronExpression.Parser
{
    public class Lexer : LexerBase<Token>
    {

        public Lexer(string input) : 
            base(input, new NoneToken(new TextSpan(0, 0)), 
                new TokenDefinition(@"[\*]{1}(?=[\s,]{1,})"),
                new TokenDefinition(@"[_]{1}(?=[\s,]{1,})"),
                new TokenDefinition(@"[?]{1}(?=[\s,]{1,})"),
                new TokenDefinition(@"[#]{1}"),
                new TokenDefinition(@"(\r\n)|(\r)|(\n)|([\s]{1})", RegexOptions.Singleline),
                new TokenDefinition(@"[\,]{1}"),
                new TokenDefinition(@"[\-]{1}"),
                new TokenDefinition(@"[\/]{1}"),
                new TokenDefinition(@"[\r\n]{1}"),
                new TokenDefinition(@"((\b\d*)(LW|L|W)(?=[\s,]{1,})|(\b\d*)(LW|L|W)$)"),
                new TokenDefinition(@"[\d]{1,}"),
                new TokenDefinition(@"\w{1,3}(?=([\s,#-]|$))"),
                new TokenDefinition(@"(([\w*?_]{1,}))", RegexOptions.Singleline))
        { }

        protected override Token GetEndOfFileToken()
        {
            return new EndOfFileToken(new TextSpan(Input.Length, 0));
        }

        protected override Token GetToken(TokenDefinition matchedDefinition, Match match)
        {
            string token = match.Value;
            int matchLength = match.Length;

            switch (GetTokenCandidate(token))
            {
                case TokenType.Hash:
                    return AssignTokenOfType(() => new HashToken(new TextSpan(Position, 1)));
                case TokenType.WhiteSpace:
                    return AssignTokenOfType(() => new WhiteSpaceToken(new TextSpan(Position, 1)));
                case TokenType.QuestionMark:
                    return AssignTokenOfType(() => new QuestionMarkToken(new TextSpan(Position, 1)));
                case TokenType.Comma:
                    return AssignTokenOfType(() => new CommaToken(new TextSpan(Position, 1)));
                case TokenType.Star:
                    return AssignTokenOfType(() => new StarToken(new TextSpan(Position, 1)));
                case TokenType.Range:
                    return AssignTokenOfType(() => new RangeToken(new TextSpan(Position, 1)));
                case TokenType.Inc:
                    return AssignTokenOfType(() => new IncrementByToken(new TextSpan(Position, 1)));
                case TokenType.Missing:
                    return AssignTokenOfType(() => new MissingToken(new TextSpan(Position, 1)));
                case TokenType.Integer:
                    return AssignTokenOfType(() => new IntegerToken(token, new TextSpan(Position, token.Length)));
            }

            var splited = matchedDefinition.Regex.Split(token).Where(f => f != token).ToArray();

            if (splited.Length > 2)
            {
                if (splited[1] == string.Empty)
                {
                    splited[1] = "1";
                }
                if (splited[2] == string.Empty)
                {
                    splited[2] = token;
                }

                switch (splited[2])
                {
                    case "L":
                        return AssignTokenOfType(() => new LToken(int.Parse(splited[1]), new TextSpan(Position, matchLength)));
                    case "LW":
                        return AssignTokenOfType(() => new LWToken(int.Parse(splited[1]), new TextSpan(Position, matchLength)));
                    case "W":
                        return AssignTokenOfType(() => new WToken(int.Parse(splited[1]), new TextSpan(Position, matchLength)));
                }
            }

            return new NameToken(token, new TextSpan(Position, matchLength));
        }

        private TokenType GetTokenCandidate(string text)
        {
            switch (text)
            {
                case "#":
                    return TokenType.Hash;
                case "?":
                    return TokenType.QuestionMark;
                case "*":
                    return TokenType.Star;
                case "-":
                    return TokenType.Range;
                case "/":
                    return TokenType.Inc;
                case " ":
                    return TokenType.WhiteSpace;
                case ",":
                    return TokenType.Comma;
                case "_":
                    return TokenType.Missing;
            }

            if(this.IsEndLine(text))
            {
                return TokenType.WhiteSpace;
            }

            int number = 0;
            if (int.TryParse(text, out number) && !text.Contains(" "))
            {
                return TokenType.Integer;
            }

            return TokenType.Name;
        }

        private bool IsEndLine(string text) => text == Environment.NewLine || text == "\r" || text == "\n";
    }
}
