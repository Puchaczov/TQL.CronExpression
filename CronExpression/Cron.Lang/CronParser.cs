using Cron.Parser.Enums;
using Cron.Parser.Exceptions;
using Cron.Parser.Nodes;
using Cron.Parser.Tokens;
using System.Collections.Generic;

namespace Cron.Parser
{
    public class CronParser
    {
        private Lexer lexer;
        private Token currentToken;
        private Token lastToken;

        public CronParser(Lexer lexer)
        {
            this.lexer = lexer;
            lastToken = new NoneToken();
            currentToken = lexer.NextToken();
        }

        private void Consume(TokenType type)
        {
            if (currentToken.TokenType == type)
            {
                lastToken = currentToken;
                currentToken = lexer.NextToken();
                return;
            }
            throw new UnexpectedTokenException(lexer.Position, currentToken);
        }

        public RootComponentNode ComposeRootComponents()
        {
            List<SegmentNode> rootComponents = new List<SegmentNode>();
            for (int i = 0; i < 8 && currentToken.TokenType != TokenType.Eof; ++i)
            {
                switch (currentToken.TokenType)
                {
                    case TokenType.WhiteSpace:
                        Consume(TokenType.WhiteSpace);
                        break;
                }
                rootComponents.Add(ComposeSegmentComponent((Segment)i));
            }
            if(rootComponents[rootComponents.Count - 1].Segment != Segment.Year)
            {
                rootComponents.Add(ComposeStarYearSegmentComponent());
            }
            if(currentToken.TokenType == TokenType.Eof)
            {
                rootComponents.Add(new EndOfFileNode());
            }
            if(rootComponents.Count != 8)
            {
                throw new MismatchedSegmentsCountException();
            }
            return new RootComponentNode(rootComponents.ToArray());
        }

        private SegmentNode ComposeStarYearSegmentComponent()
        {
            return new SegmentNode(new StarNode(Segment.Year), Segment.Year);
        }

        private SegmentNode ComposeSegmentComponent(Segment segment)
        {
            switch (segment)
            {
                case Segment.Seconds:
                    return ComposeComplexSegment(segment);
                case Segment.Minutes:
                    return ComposeComplexSegment(segment);
                case Segment.Hours:
                    return ComposeComplexSegment(segment);
                case Segment.DayOfMonth:
                    return ComposeComplexSegment(segment);
                case Segment.Month:
                    return ComposeComplexSegment(segment);
                case Segment.DayOfWeek:
                    return ComposeComplexSegment(segment);
                case Segment.Year:
                    return ComposeComplexSegment(segment);
                default:
                    throw new UnknownSegmentException(lexer.Position);
            }
        }

        private SyntaxOperatorNode TakePrimitiveInteger()
        {
            var token = currentToken;
            Consume(currentToken.TokenType);
            switch (currentToken.TokenType)
            {
                case TokenType.L:
                    Consume(TokenType.L);
                    return new NumericPrecededLNode(token);
                case TokenType.W:
                    Consume(TokenType.W);
                    return new NumericPrecededWNode(token);
                case TokenType.LW:
                    Consume(TokenType.LW);
                    return new NumericPreceededLWNode(token);
                default:
                    return new NumberNode(lastToken);
            }
        }

        private SyntaxOperatorNode TakePrimitives()
        {
            var token = currentToken;
            switch (currentToken.TokenType)
            {
                case TokenType.Integer:
                    return TakePrimitiveInteger();
                case TokenType.Name:
                    Consume(TokenType.Name);
                    return new WordNode(token);
                case TokenType.L:
                    Consume(TokenType.L);
                    return new LNode();
                case TokenType.W:
                    Consume(TokenType.W);
                    return new WNode();
                case TokenType.LW:
                    Consume(TokenType.LW);
                    return new LWNode();
            }
            throw new NestedExpressionException(lexer.Position, token);
        }

        private SyntaxOperatorNode TakeComplex()
        {
            var node = TakePrimitives();

            while (currentToken.TokenType == TokenType.Range || currentToken.TokenType == TokenType.Inc || currentToken.TokenType == TokenType.Hash)
            {
                var token = currentToken;
                Consume(currentToken.TokenType);

                switch (token.TokenType)
                {
                    case TokenType.Range:
                        node = new RangeNode(node, TakePrimitives());
                        break;
                    case TokenType.Inc:
                        node = new IncrementByNode(node, TakePrimitives());
                        break;
                    case TokenType.Hash:
                        node = new HashNode(node.Token, TakePrimitives().Token);
                        break;
                }
            }

            return node;
        }

        private SyntaxOperatorNode SeparateCommas()
        {
            var node = TakeComplex();
            while (currentToken.TokenType == TokenType.Comma)
            {
                switch (currentToken.TokenType)
                {
                    case TokenType.Comma:
                        Consume(TokenType.Comma);
                        break;
                }

                node = new CommaNode(node, TakeComplex());
            }
            return node;
        }

        private SegmentNode ComposeComplexSegment(Segment segment)
        {
            switch (currentToken.TokenType)
            {
                case TokenType.Star:
                    Consume(TokenType.Star);
                    return new SegmentNode(new StarNode(segment), segment);
                case TokenType.QuestionMark:
                    Consume(TokenType.QuestionMark);
                    return new SegmentNode(new QuestionMarkNode(), segment);
                default:
                    return new SegmentNode(SeparateCommas(), segment);
            }
        }
    }
}
