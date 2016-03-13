using Cron.Parser.Enums;
using Cron.Parser.Tokens;
using Cron.Parser.Visitors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cron.Parser.Nodes
{
    public class NumberNode : LeafNode
    {
        public NumberNode(Token token)
            : base(token)
        { }

        public override void Accept(INodeVisitor visitor)
        {
            visitor.Visit(this);
        }

        public override IList<int> Evaluate(Segment segment)
        {
            return new List<int> { int.Parse(Token.Value) };
        }

        public override SyntaxNode[] Desecendants
        {
            get
            {
                return new SyntaxNode[0];
            }
        }

        public override string ToString()
        {
            return Token.Value;
        }
    }
}
