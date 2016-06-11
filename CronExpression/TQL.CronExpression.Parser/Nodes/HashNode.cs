using System.Collections.Generic;
using TQL.CronExpression.Parser.Enums;
using TQL.CronExpression.Parser.Extensions;
using TQL.CronExpression.Parser.Tokens;
using TQL.CronExpression.Parser.Visitors;

namespace TQL.CronExpression.Parser.Nodes
{
    public class HashNode : BinaryExpressionNode
    {
        private readonly CronSyntaxNode left;
        private readonly CronSyntaxNode right;

        public HashNode(CronSyntaxNode left, CronSyntaxNode right, Token token)
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

        public override IList<int> Evaluate(Segment segment) => ListExtension.Empty();

        public override string ToString() => Left.ToString() + Token.Value + Right.ToString();
    }
}
