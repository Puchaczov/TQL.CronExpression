using Cron.Parser.Enums;
using Cron.Parser.Extensions;
using Cron.Parser.Tokens;
using Cron.Parser.Visitors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cron.Parser.Nodes
{
    public class HashNode : BinaryExpressionNode
    {
        private readonly SyntaxNode left;
        private readonly SyntaxNode right;

        public HashNode(SyntaxNode left, SyntaxNode right, Token token)
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

        public override IList<int> Evaluate(Segment segment) => ListExtension.Empty();

        public override string ToString() => Left.ToString() + Token.Value + Right.ToString();
    }
}
