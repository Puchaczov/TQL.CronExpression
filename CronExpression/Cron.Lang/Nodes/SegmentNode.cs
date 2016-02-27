using Cron.Parser.Enums;
using Cron.Parser.Tokens;
using Cron.Parser.Visitors;
using System;
using System.Collections.Generic;

namespace Cron.Parser.Nodes
{
    public class SegmentNode : SyntaxOperatorNode
    {
        private SyntaxOperatorNode node;
        private Segment segment;

        public SegmentNode(SyntaxOperatorNode segmentTree, Segment segment)
        {
            this.node = segmentTree;
            this.segment = segment;
        }

        public SyntaxOperatorNode Node
        {
            get
            {
                return node;
            }
        }

        public Segment Segment
        {
            get
            {
                return segment;
            }
        }

        public override void Accept(INodeVisitor visitor)
        {
            visitor.Visit(this);
            node.Accept(visitor);
        }

        public override IList<int> Evaluate(Segment segment)
        {
            return node.Evaluate(segment);
        }

        public override SyntaxNode[] Items
        {
            get
            {
                return new SyntaxNode[] {
                    node
                };
            }
        }

        public override Token Token
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public override string ToString()
        {
            return node.ToString();
        }
    }
}
