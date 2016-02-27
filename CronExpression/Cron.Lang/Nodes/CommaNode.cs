using Cron.Parser.Enums;
using Cron.Parser.Tokens;
using Cron.Parser.Visitors;
using System.Collections.Generic;

namespace Cron.Parser.Nodes
{
    public class CommaNode : SyntaxOperatorNode
    {
        private SyntaxOperatorNode left;
        private SyntaxOperatorNode right;

        public CommaNode(SyntaxOperatorNode left, SyntaxOperatorNode right)
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
            foreach(var item in Items)
            {
                list.AddRange(item.Evaluate(segment));
            }
            return list;
        }

        public override SyntaxNode[] Items
        {
            get
            {
                List<SyntaxOperatorNode> commaItems = new List<SyntaxOperatorNode>();
                SyntaxOperatorNode current = left;
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
                return commaItems.ToArray();
            }
        }

        public override Token Token
        {
            get
            {
                return new CommaToken();
            }
        }

        public SyntaxOperatorNode Left
        {
            get
            {
                return left;
            }
        }

        public SyntaxOperatorNode Right
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