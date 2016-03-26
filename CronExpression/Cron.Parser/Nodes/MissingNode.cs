using Cron.Parser.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cron.Parser.Enums;
using Cron.Parser.Visitors;

namespace Cron.Parser.Nodes
{
    public class MissingNode : LeafNode
    {
        public MissingNode(Token token)
            : base(token)
        { }

        public override SyntaxNode[] Desecendants
        {
            get
            {
                return new SyntaxNode[0];
            }
        }

        public override void Accept(INodeVisitor visitor)
        {
            visitor.Visit(this);
        }

        public override IList<int> Evaluate(Segment segment)
        {
            throw new Exception("Cannot evaluate missing node");
        }

        public override string ToString()
        {
            return this.Token.Value;
        }
    }
}
