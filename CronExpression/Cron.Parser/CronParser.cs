using Cron.Parser.Enums;
using Cron.Parser.Exceptions;
using Cron.Parser.Nodes;
using Cron.Parser.Tokens;
using System.Collections.Generic;

namespace Cron.Parser
{
    public class CronParser
    {
        private Segment currentSegment;
        private Token currentToken;
        private Token lastToken;
        private readonly Lexer lexer;
        private readonly bool produceEndOfFileNode = true;

        private readonly bool produceMissingYearSegment = true;
        private readonly bool produceMissingSecondSegment;

        public CronParser(Lexer lexer, bool produceMissingYearSegment, bool produceEndOfFileNode, bool produceMissingSecondSegment)
            : this(lexer)
        {
            this.produceMissingYearSegment = produceMissingYearSegment;
            this.produceMissingSecondSegment = produceMissingSecondSegment;
            this.produceEndOfFileNode = produceEndOfFileNode;
        }

        public CronParser(Lexer lexer)
        {
            this.lexer = lexer;
            lastToken = new NoneToken(new TextSpan(0, 0));
            currentToken = lexer.NextToken();
        }

        public bool WithYearComponentWhenMissing => produceMissingYearSegment;

        public bool WithSecondsComponentWhenMissing => produceMissingSecondSegment;

        public RootComponentNode ComposeRootComponents()
        {
            var rootComponents = new List<SegmentNode>();
            var i = 0;

            if (produceMissingSecondSegment)
            {
                rootComponents.Add(ComposeValueBasedSecondsSegmentComponent());
                i += 1;
            }
            for (; currentToken.TokenType != TokenType.Eof; ++i)
            {
                while (currentToken.TokenType == TokenType.WhiteSpace || currentToken.TokenType == TokenType.NewLine)
                {
                    Consume(currentToken.TokenType);
                }
                rootComponents.Add(ComposeSegmentComponent((Segment)i));
            }
            if (produceMissingYearSegment && rootComponents[rootComponents.Count - 1].Segment == Segment.DayOfWeek)
            {
                rootComponents.Add(ComposeStarYearSegmentComponent());
            }
            if (produceEndOfFileNode && currentToken.TokenType == TokenType.Eof)
            {
                rootComponents.Add(new EndOfFileNode(new EndOfFileToken(currentToken.Span)));
            }
            return new RootComponentNode(rootComponents.ToArray());
        }

        private SegmentNode ComposeComplexSegment(Segment segment) => new SegmentNode(SeparateCommas(), segment, null);

        private LeafNode ComposeMissingNodeOnCurrentPosition() => new MissingNode(new MissingToken(new TextSpan(lexer.Position, 0)));

        private SegmentNode ComposeSegmentComponent(Segment segment)
        {
            switch (segment)
            {
                case Segment.Seconds:
                    currentSegment = Segment.Seconds;
                    break;
                case Segment.Minutes:
                    currentSegment = Segment.Minutes;
                    break;
                case Segment.Hours:
                    currentSegment = Segment.Hours;
                    break;
                case Segment.DayOfMonth:
                    currentSegment = Segment.DayOfMonth;
                    break;
                case Segment.Month:
                    currentSegment = Segment.Month;
                    break;
                case Segment.DayOfWeek:
                    currentSegment = Segment.DayOfWeek;
                    break;
                case Segment.Year:
                    currentSegment = Segment.Year;
                    break;
                default:
                    currentSegment = Segment.Unknown;
                    break;
            }
            return ComposeComplexSegment(segment);
        }

        private SegmentNode ComposeStarYearSegmentComponent() => new SegmentNode(new StarNode(Segment.Year, new StarToken(new TextSpan(lexer.Position, 0))), Segment.Year, null);

        private SegmentNode ComposeValueBasedSecondsSegmentComponent() => new SegmentNode(new NumberNode(new IntegerToken("0", new TextSpan(lexer.Position, 1))), Segment.Seconds, null);

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

        private SyntaxNode SeparateCommas()
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

                var comma = lastToken;
                node = new CommaNode(node, TakeComplex(), comma);
            }
            return node;
        }

        private SyntaxNode TakeComplex()
        {
            SyntaxNode node = TakePrimitives();

            while (currentToken.TokenType == TokenType.Range || currentToken.TokenType == TokenType.Inc || currentToken.TokenType == TokenType.Hash)
            {
                var token = currentToken;
                Consume(currentToken.TokenType);

                switch (token.TokenType)
                {
                    case TokenType.Range:
                        node = new RangeNode(node, TakePrimitives(), token);
                        break;
                    case TokenType.Inc:
                        node = new IncrementByNode(node, TakePrimitives(), token);
                        break;
                    case TokenType.Hash:
                        node = new HashNode(node, TakePrimitives(), token);
                        break;
                }
            }

            return node;
        }

        private LeafNode TakePrimitiveInteger()
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
                default:
                    return new NumberNode(lastToken);
            }
        }

        private LeafNode TakePrimitives()
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
                    return new LNode(token);
                case TokenType.W:
                    Consume(TokenType.W);
                    return new WNode(token);
                case TokenType.LW:
                    Consume(TokenType.LW);
                    return new LWNode(token);
                case TokenType.Star:
                    Consume(TokenType.Star);
                    return new StarNode(currentSegment, token);
                case TokenType.QuestionMark:
                    Consume(TokenType.QuestionMark);
                    return new QuestionMarkNode(token);
                case TokenType.Missing:
                    Consume(TokenType.Missing);
                    return ComposeMissingNodeOnCurrentPosition();
            }
            return ComposeMissingNodeOnCurrentPosition();
        }
    }
}
