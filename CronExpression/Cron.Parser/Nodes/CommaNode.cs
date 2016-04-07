using Cron.Parser.Enums;
using Cron.Parser.Tokens;
using Cron.Parser.Visitors;
using System.Collections.Generic;
using System.Linq;

namespace Cron.Parser.Nodes
{
    public class CommaNode : BinaryExpressionNode
    {
        private SyntaxNode left;
        private SyntaxNode right;

        public CommaNode(SyntaxNode left, SyntaxNode right, Token token)
            : base(token)
        {
            this.left = left;
            this.right = right;
        }

        public override void Accept(INodeVisitor visitor)
        {
            visitor.Visit(this);
            left.Accept(visitor);
            right.Accept(visitor);
        }

        public override IList<int> Evaluate(Segment segment)
        {
            var list = new List<int>();
            foreach(var item in Desecendants)
            {
                list.AddRange(item.Evaluate(segment));
            }
            return list;
        }

        public override SyntaxNode[] Desecendants
        {
            get
            {
                var commaItems = new List<SyntaxNode>();
                var current = left;
                commaItems.Add(right);
                while(current != null)
                {
                    var commaCurrent = current as CommaNode;
                    if(commaCurrent != null)
                    {
                        current = commaCurrent.Left;
                        commaItems.Add(commaCurrent.Right);
                    }
                    else
                    {
                        commaItems.Add(current);
                        current = null;
                    }
                }
                commaItems.Reverse();
                return commaItems.ToArray();
            }
        }

        public override TextSpan FullSpan
        {
            get
            {
                var items = Desecendants;
                var stop = items.Last().FullSpan.End;
                var start = items.First().FullSpan.Start;
                return new TextSpan(start, stop - start);
            }
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

        public override string ToString()
        {
            return left.ToString() + Token.Value + right.ToString();
        }
    }
}