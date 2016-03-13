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
        private SyntaxNode left;
        private SyntaxNode right;

        public HashNode(SyntaxNode left, SyntaxNode right, Token token)
            : base(token)
        {
            this.left = left;
            this.right = right;
        }

        public override void Accept(INodeVisitor visitor)
        {
            visitor.Visit(this);
        }

        public override IList<int> Evaluate(Segment segment)
        {
            return ListExtension.Empty();
        }

        public override SyntaxNode Left
        {
            get
            {
                return left;
            }
        }

        public override SyntaxNode Right
        {
            get
            {
                return right;
            }
        }

        public override SyntaxNode[] Desecendants
        {
            get
            {
                return new SyntaxNode[] {
                    left,
                    right
                };
            }
        }

        public override string ToString()
        {
            return Left.ToString() + Token.Value + Right.ToString();
        }
    }
}
