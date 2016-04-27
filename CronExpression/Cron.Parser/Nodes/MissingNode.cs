using Cron.Parser.Tokens;
using System;
using System.Collections.Generic;
using Cron.Parser.Enums;
using Cron.Parser.Visitors;

namespace Cron.Parser.Nodes
{
    public class MissingNode : LeafNode
    {
        public MissingNode(Token token)
            : base(token)
        { }

        public override SyntaxNode[] Desecendants => new SyntaxNode[0];

        public override void Accept(INodeVisitor visitor)
        {
            visitor.Visit(this);
        }

        public override IList<int> Evaluate(Segment segment)
        {
            throw new Exception("Cannot evaluate missing node");
        }

        public override string ToString() => this.Token.Value;
    }
}
