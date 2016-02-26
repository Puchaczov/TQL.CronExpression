using Cron.Parser.Tokens;
using Cron.Parser.Visitors;
using System;

namespace Cron.Parser.Syntax
{
    public class EndOfFileNode : SegmentNode
    {
        public EndOfFileNode()
            : base(null, 0)
        { }

        public override SyntaxNode[] Items
        {
            get
            {
                return new SyntaxNode[0];
            }
        }

        public override Token Token
        {
            get
            {
                return new EndOfFileToken();
            }
        }

        public override void Accept(INodeVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}