using Cron.Parser.Tokens;
using System.Collections.Generic;
using Cron.Parser.Enums;
using Cron.Parser.Visitors;
using Cron.Parser.Exceptions;

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
            throw new EvaluationException(segment, "Cannot evaluate missing node.");
        }

        public override string ToString() => this.Token.Value;
    }
}
