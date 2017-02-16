using System.Collections.Generic;
using System.Linq;
using TQL.CronExpression.Parser.Enums;
using TQL.CronExpression.Parser.Exceptions;
using TQL.CronExpression.Parser.Extensions;
using TQL.CronExpression.Parser.Tokens;

namespace TQL.CronExpression.Parser.Nodes
{
    public class IncrementByNode : BinaryExpressionNode
    {
        private readonly CronSyntaxNode _left;
        private readonly CronSyntaxNode _right;

        public IncrementByNode(CronSyntaxNode left, CronSyntaxNode right, Token token)
            : base(token)
        {
            this._left = left;
            this._right = right;
        }

        public override CronSyntaxNode[] Desecendants => new[]
        {
            _left,
            _right
        };

        public override CronSyntaxNode Left => _left;

        public override CronSyntaxNode Right => _right;

        public override void Accept(INodeVisitor visitor)
        {
            visitor.Visit(this);
        }

        public override IList<int> Evaluate(Segment segment)
        {
            switch (_left.Token.TokenType)
            {
                case TokenType.Range:
                    return _left.Evaluate(segment)
                        .CutMe(_left.Desecendants[0].Token.Value, _left.Desecendants[1].Token.Value, _right.Token.Value);
                case TokenType.Name:
                case TokenType.Integer:
                    switch (segment)
                    {
                        case Segment.DayOfWeek:
                            return ListExtension.Expand(_left.Evaluate(segment).First(), 7,
                                _right.Evaluate(segment).First());
                        case Segment.Month:
                            return ListExtension.Expand(_left.Evaluate(segment).First(), 12,
                                _right.Evaluate(segment).First());
                        case Segment.Year:
                            return ListExtension.Expand(_left.Evaluate(segment).First(), 3000,
                                _right.Evaluate(segment).First());
                        case Segment.DayOfMonth:
                            return ListExtension.Expand(_left.Evaluate(segment).First(), 32,
                                _right.Evaluate(segment).First());
                        case Segment.Hours:
                            return ListExtension.Expand(_left.Evaluate(segment).First(), 23,
                                _right.Evaluate(segment).First());
                        case Segment.Minutes:
                            return ListExtension.Expand(_left.Evaluate(segment).First(), 59,
                                _right.Evaluate(segment).First());
                        case Segment.Seconds:
                            return ListExtension.Expand(_left.Evaluate(segment).First(), 59,
                                _right.Evaluate(segment).First());
                    }
                    throw new UnexpectedSegmentException(segment);
                default:
                    throw new UnexpectedTokenException(0, _left.Token);
            }
        }

        public override string ToString() => Left + Token.Value + Right;
    }
}