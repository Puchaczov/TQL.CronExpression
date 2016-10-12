using TQL.Core.Syntax;
using TQL.Core.Tokens;
using System;
using TQL.CronExpression.Parser.Tokens;
using TQL.CronExpression.Parser.Exceptions;
using TQL.CronExpression.Parser.Enums;
using System.Text.RegularExpressions;
using System.Linq;

namespace TQL.CronExpression.Parser
{
    /// <summary>
    /// Idea how to implement this piece of code where founded here:
    /// https://blogs.msdn.microsoft.com/drew/2009/12/31/a-simple-lexer-in-c-that-uses-regular-expressions/
    /// </summary>
    public class Lexer : LexerBase<Token>
    {
        private class TokenDefinition
        {
            public Regex Regex { get; }

            public TokenDefinition(string pattern)
            {
                Regex = new Regex(pattern);
            }

            public TokenDefinition(string pattern, RegexOptions options)
            {
                Regex = new Regex(pattern, options);
            }
        }

        private class TokenPosition
        {
            public int Index { get; }
            public int Length { get; }

            public TokenPosition(int index, int length)
            {
                this.Index = index;
                this.Length = length;
            }
        }


        private TokenDefinition[] definitions;

        public Lexer(string input) : 
            base(input, new NoneToken(new TextSpan(0, 0)))
        {
            definitions = new TokenDefinition[] {
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
                new TokenDefinition(@"\w{1,3}"),
                new TokenDefinition(@"(([\w*?_]{1,}))", RegexOptions.Singleline),
            };
        }

        public override Token NextToken()
        {
            while(!IsOutOfRange)
            {
                TokenDefinition matchedDefinition = null;
                int matchLength = 0;
                
                foreach(var rule in definitions)
                {
                    var match = rule.Regex.Match(input, pos);

                    if(match.Success && match.Index - pos == 0)
                    {
                        matchedDefinition = rule;
                        matchLength = match.Length;
                        break;
                    }
                }

                if(matchedDefinition == null)
                {
                    throw new UnknownTokenException(pos, input[pos], string.Format("Unrecognized token exception at {0} for {1}", pos, input.Substring(pos)));
                }
                else
                {
                    var value = input.Substring(pos, matchLength);

                    var oldPos = pos;
                    pos += matchLength;

                    switch (GetTokenCandidate(value))
                    {
                        case TokenType.Hash:
                            return AssignTokenOfType(() => new HashToken(new TextSpan(oldPos, 1)));
                        case TokenType.WhiteSpace:
                            return AssignTokenOfType(() => new WhiteSpaceToken(new TextSpan(oldPos, 1)));
                        case TokenType.QuestionMark:
                            return AssignTokenOfType(() => new QuestionMarkToken(new TextSpan(oldPos, 1)));
                        case TokenType.Comma:
                            return AssignTokenOfType(() => new CommaToken(new TextSpan(oldPos, 1)));
                        case TokenType.Star:
                            return AssignTokenOfType(() => new StarToken(new TextSpan(oldPos, 1)));
                        case TokenType.Range:
                            return AssignTokenOfType(() => new RangeToken(new TextSpan(oldPos, 1)));
                        case TokenType.Inc:
                            return AssignTokenOfType(() => new IncrementByToken(new TextSpan(oldPos, 1)));
                        case TokenType.Missing:
                            return AssignTokenOfType(() => new MissingToken(new TextSpan(oldPos, 1)));
                        case TokenType.Integer:
                            return AssignTokenOfType(() => new IntegerToken(value, new TextSpan(oldPos, value.Length)));
                    }

                    var splited = matchedDefinition.Regex.Split(value).Where(f => f != value).ToArray();

                    if(splited.Length > 2)
                    {
                        if (splited[1] == string.Empty)
                        {
                            splited[1] = "1";
                        }
                        if(splited[2] == string.Empty)
                        {
                            splited[2] = value;
                        }

                        switch (splited[2])
                        {
                            case "L":
                                return AssignTokenOfType(() => new LToken(int.Parse(splited[1]), new TextSpan(oldPos, matchLength)));
                            case "LW":
                                return AssignTokenOfType(() => new LWToken(int.Parse(splited[1]), new TextSpan(oldPos, matchLength)));
                            case "W":
                                return AssignTokenOfType(() => new WToken(int.Parse(splited[1]), new TextSpan(oldPos, matchLength)));
                        }
                    }

                    return AssignTokenOfType(() => new NameToken(value, new TextSpan(oldPos, matchLength)));

                }
            }

            return AssignTokenOfType(() => new EndOfFileToken(new TextSpan(pos, 0)));
        }

        private bool IsOutOfRange => pos >= input.Length;
        
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

        private bool IsEndLine(string text)
        {
            return text == Environment.NewLine || text == "\r" || text == "\n";
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
