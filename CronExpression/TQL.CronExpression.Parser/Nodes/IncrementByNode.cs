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
        private readonly CronSyntaxNode left;
        private readonly CronSyntaxNode right;

        public IncrementByNode(CronSyntaxNode left, CronSyntaxNode right, Token token)
            : base(token)
        {
            this.left = left;
            this.right = right;
        }

        public override CronSyntaxNode[] Desecendants => new CronSyntaxNode[] {
                    left,
                    right
                };

        public override CronSyntaxNode Left => left;

        public override CronSyntaxNode Right => right;

        public override void Accept(INodeVisitor visitor)
        {
            visitor.Visit(this);
        }

        public override IList<int> Evaluate(Segment segment)
        {
            switch(left.Token.TokenType)
            {
                case TokenType.Range:
                    return left.Evaluate(segment).CutMe(left.Desecendants[0].Token.Value, left.Desecendants[1].Token.Value, right.Token.Value);
                case TokenType.Name:
                case TokenType.Integer:
                    switch(segment)
                    {
                        case Segment.DayOfWeek:
                            return ListExtension.Expand(left.Evaluate(segment).First(), 7, right.Evaluate(segment).First());
                        case Segment.Month:
                            return ListExtension.Expand(left.Evaluate(segment).First(), 12, right.Evaluate(segment).First());
                        case Segment.Year:
                            return ListExtension.Expand(left.Evaluate(segment).First(), 3000, right.Evaluate(segment).First());
                        case Segment.DayOfMonth:
                            return ListExtension.Expand(left.Evaluate(segment).First(), 32, right.Evaluate(segment).First());
                        case Segment.Hours:
                            return ListExtension.Expand(left.Evaluate(segment).First(), 23, right.Evaluate(segment).First());
                        case Segment.Minutes:
                            return ListExtension.Expand(left.Evaluate(segment).First(), 59, right.Evaluate(segment).First());
                        case Segment.Seconds:
                            return ListExtension.Expand(left.Evaluate(segment).First(), 59, right.Evaluate(segment).First());
                    }
                    throw new UnexpectedSegmentException(segment);
                default:
                    throw new UnexpectedTokenException(0, left.Token);
            }
        }

        public override string ToString() => Left.ToString() + Token.Value + Right.ToString();
    }
}
