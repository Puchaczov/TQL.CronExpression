using Cron.Parser.Tokens;
using Cron.Parser.Visitors;
using System;

namespace Cron.Parser.Nodes
{
    public class EndOfFileNode : SegmentNode
    {
        public EndOfFileNode(Token token)
            : base(null, 0, token)
        { }

        public override SyntaxNode[] Desecendants => new SyntaxNode[0];

        public override TextSpan FullSpan => Token.Span;

        public override void Accept(INodeVisitor visitor)
        {
            visitor.Visit(this);
        }

        public override string ToString() => string.Empty;
    }
}