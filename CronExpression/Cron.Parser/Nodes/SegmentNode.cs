using Cron.Parser.Enums;
using Cron.Parser.Tokens;
using Cron.Parser.Visitors;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Cron.Parser.Nodes
{
    [DebuggerDisplay("{GetType().Name,nq}: {ToString(),nq}")]
    public class SegmentNode : UnaryExpressionNode
    {
        private readonly SyntaxNode node;
        private readonly Segment segment;
        private readonly Token token;

        public SegmentNode(SyntaxNode segmentTree, Segment segment, Token token)
        {
            this.node = segmentTree;
            this.segment = segment;
            this.token = token;
        }

        public override SyntaxNode Descendant => node;

        public override SyntaxNode[] Desecendants
        {
            get
            {
                switch (node.Token.TokenType)
                {
                    case TokenType.Comma:
                        return node.Desecendants;
                    default:
                        return new SyntaxNode[] {
                            node
                        };
                }
            }
        }

        public override TextSpan FullSpan
        {
            get
            {
                var descs = this.Desecendants;
                var start = descs[0].FullSpan;
                var stop = descs[descs.Length - 1].FullSpan;
                return new TextSpan(start.Start, stop.End - start.Start);
            }
        }

        public Segment Segment => segment;

        public override Token Token => token;

        public override void Accept(INodeVisitor visitor)
        {
            visitor.Visit(this);
            node.Accept(visitor);
        }

        public override IList<int> Evaluate(Segment segment) => node.Evaluate(segment);

        public override string ToString() => node.ToString();
    }
}
