using Cron.Parser.Enums;
using Cron.Parser.Tokens;
using Cron.Parser.Visitors;
using System.Collections.Generic;

namespace Cron.Parser.Nodes
{
    public class NumberNode : LeafNode
    {
        public NumberNode(Token token)
            : base(token)
        { }

        public override SyntaxNode[] Desecendants => new SyntaxNode[0];

        public override void Accept(INodeVisitor visitor)
        {
            visitor.Visit(this);
        }

        public override IList<int> Evaluate(Segment segment) => new List<int> { int.Parse(Token.Value) };

        public override string ToString() => Token.Value;
    }
}
