using Cron.Parser.Visitors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cron.Parser.Tokens;
using Cron.Parser.Extensions;
using Cron.Parser.Enums;
using Cron.Parser.Exceptions;

namespace Cron.Parser.Nodes
{
    public class IncrementByNode : BinaryExpressionNode
    {
        private readonly SyntaxNode left;
        private readonly SyntaxNode right;

        public IncrementByNode(SyntaxNode left, SyntaxNode right, Token token)
            : base(token)
        {
            this.left = left;
            this.right = right;
        }

        public override SyntaxNode[] Desecendants => new SyntaxNode[] {
                    left,
                    right
                };

        public override SyntaxNode Left => left;

        public override SyntaxNode Right => right;

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
