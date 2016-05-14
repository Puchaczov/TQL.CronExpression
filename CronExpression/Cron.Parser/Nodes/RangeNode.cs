using Cron.Parser.Tokens;
using Cron.Parser.Visitors;
using System.Collections.Generic;
using Cron.Parser.Enums;
using Cron.Parser.Extensions;

namespace Cron.Parser.Nodes
{
    public class RangeNode : BinaryExpressionNode
    {
        private readonly CronSyntaxNode left;
        private readonly CronSyntaxNode right;

        public RangeNode(CronSyntaxNode left, CronSyntaxNode right, Token token)
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
