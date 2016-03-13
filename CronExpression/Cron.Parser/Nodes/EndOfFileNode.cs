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

        public override string ToString()
        {
            return string.Empty;
        }

        public override TextSpan FullSpan
        {
            get
            {
                return Token.Span;
            }
        }
    }
}