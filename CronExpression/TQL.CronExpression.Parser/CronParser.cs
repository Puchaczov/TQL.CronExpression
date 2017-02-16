using System.Collections.Generic;
using TQL.Core.Tokens;
using TQL.CronExpression.Parser.Enums;
using TQL.CronExpression.Parser.Exceptions;
using TQL.CronExpression.Parser.Nodes;
using TQL.CronExpression.Parser.Tokens;

namespace TQL.CronExpression.Parser
{
    public class CronParser
    {
        private readonly Lexer _lexer;
        private readonly bool _produceEndOfFileNode = true;

        private Segment _currentSegment;
        private Token _currentToken;
        private Token _lastToken;

        public CronParser(Lexer lexer, bool produceMissingYearSegment, bool produceEndOfFileNode,
            bool produceMissingSecondSegment)
            : this(lexer)
        {
            WithYearComponentWhenMissing = produceMissingYearSegment;
            WithSecondsComponentWhenMissing = produceMissingSecondSegment;
            this._produceEndOfFileNode = produceEndOfFileNode;
        }

        public CronParser(Lexer lexer)
        {
            this._lexer = lexer;
            _lastToken = new NoneToken(new TextSpan(0, 0));
            _currentToken = lexer.Next();
        }

        public bool WithYearComponentWhenMissing { get; } = true;

        public bool WithSecondsComponentWhenMissing { get; }

        public RootComponentNode ComposeRootComponents()
        {
            var rootComponents = new List<SegmentNode>();
            var i = 0;

            if (WithSecondsComponentWhenMissing)
            {
                rootComponents.Add(ComposeValueBasedSecondsSegmentComponent());
                i += 1;
            }
            for (; _currentToken.TokenType != TokenType.Eof; ++i)
            {
                while (_currentToken.TokenType == TokenType.WhiteSpace || _currentToken.TokenType == TokenType.NewLine)
                    Consume(_currentToken.TokenType);
                rootComponents.Add(ComposeSegmentComponent((Segment) i));
            }
            if (WithYearComponentWhenMissing && rootComponents[rootComponents.Count - 1].Segment == Segment.DayOfWeek)
                rootComponents.Add(ComposeStarYearSegmentComponent());
            if (_produceEndOfFileNode && _currentToken.TokenType == TokenType.Eof)
                rootComponents.Add(new EndOfFileNode(new EndOfFileToken(_currentToken.Span)));
            return new RootComponentNode(rootComponents.ToArray());
        }

        private SegmentNode ComposeComplexSegment(Segment segment) => new SegmentNode(SeparateCommas(), segment, null);

        private LeafNode ComposeMissingNodeOnCurrentPosition()
            => new MissingNode(new MissingToken(new TextSpan(_lexer.Position, 0)));

        private SegmentNode ComposeSegmentComponent(Segment segment)
        {
            switch (segment)
            {
                case Segment.Seconds:
                    _currentSegment = Segment.Seconds;
                    break;
                case Segment.Minutes:
                    _currentSegment = Segment.Minutes;
                    break;
                case Segment.Hours:
                    _currentSegment = Segment.Hours;
                    break;
                case Segment.DayOfMonth:
                    _currentSegment = Segment.DayOfMonth;
                    break;
                case Segment.Month:
                    _currentSegment = Segment.Month;
                    break;
                case Segment.DayOfWeek:
                    _currentSegment = Segment.DayOfWeek;
                    break;
                case Segment.Year:
                    _currentSegment = Segment.Year;
                    break;
                default:
                    _currentSegment = Segment.Unknown;
                    break;
            }
            return ComposeComplexSegment(segment);
        }

        private SegmentNode ComposeStarYearSegmentComponent()
            =>
                new SegmentNode(new StarNode(Segment.Year, new StarToken(new TextSpan(_lexer.Position, 0))), Segment.Year,
                    null);

        private SegmentNode ComposeValueBasedSecondsSegmentComponent()
            =>
                new SegmentNode(new NumberNode(new IntegerToken("0", new TextSpan(_lexer.Position, 1))), Segment.Seconds,
                    null);

        private void Consume(TokenType type)
        {
            if (_currentToken.TokenType == type)
            {
                _lastToken = _currentToken;
                _currentToken = _lexer.Next();
                return;
            }
            throw new UnexpectedTokenException(_lexer.Position, _currentToken);
        }

        private CronSyntaxNode SeparateCommas()
        {
            var node = TakeComplex();
            while (_currentToken.TokenType == TokenType.Comma)
            {
                switch (_currentToken.TokenType)
                {
                    case TokenType.Comma:
                        Consume(TokenType.Comma);
                        break;
                }

                var comma = _lastToken;
                node = new CommaNode(node, TakeComplex(), comma);
            }
            return node;
        }

        private CronSyntaxNode TakeComplex()
        {
            CronSyntaxNode node = TakePrimitives();

            while (_currentToken.TokenType == TokenType.Range || _currentToken.TokenType == TokenType.Inc ||
                   _currentToken.TokenType == TokenType.Hash)
            {
                var token = _currentToken;
                Consume(_currentToken.TokenType);

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
            var token = _currentToken;
            Consume(_currentToken.TokenType);
            switch (_currentToken.TokenType)
            {
                case TokenType.L:
                    Consume(TokenType.L);
                    return new NumericPrecededLNode(token);
                case TokenType.W:
                    Consume(TokenType.W);
                    return new NumericPrecededWNode(token);
                default:
                    return new NumberNode(_lastToken);
            }
        }

        private LeafNode TakePrimitives()
        {
            var token = _currentToken;
            switch (_currentToken.TokenType)
            {
                case TokenType.Integer:
                    return TakePrimitiveInteger();
                case TokenType.Name:
                    Consume(TokenType.Name);
                    return new WordNode(token);
                case TokenType.L:
                    Consume(TokenType.L);
                    return new LNode(token as LToken);
                case TokenType.W:
                    Consume(TokenType.W);
                    return new WNode(token as WToken);
                case TokenType.Lw:
                    Consume(TokenType.Lw);
                    return new LwNode(token as LwToken);
                case TokenType.Star:
                    Consume(TokenType.Star);
                    return new StarNode(_currentSegment, token);
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