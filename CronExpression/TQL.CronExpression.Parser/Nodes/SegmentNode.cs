using System.Collections.Generic;
using System.Diagnostics;
using TQL.Core.Tokens;
using TQL.CronExpression.Parser.Enums;
using TQL.CronExpression.Parser.Tokens;

namespace TQL.CronExpression.Parser.Nodes
{
    [DebuggerDisplay("{GetType().Name,nq}: {ToString(),nq}")]
    public class SegmentNode : UnaryExpressionNode
    {
        private readonly CronSyntaxNode _node;

        public SegmentNode(CronSyntaxNode segmentTree, Segment segment, Token token)
        {
            _node = segmentTree;
            Segment = segment;
            Token = token;
        }

        public override CronSyntaxNode Descendant => _node;

        public override CronSyntaxNode[] Desecendants
        {
            get
            {
                switch (_node.Token.TokenType)
                {
                    case TokenType.Comma:
                        return _node.Desecendants;
                    default:
                        return new[]
                        {
                            _node
                        };
                }
            }
        }

        public override TextSpan FullSpan
        {
            get
            {
                var descs = Desecendants;
                var start = descs[0].FullSpan;
                var stop = descs[descs.Length - 1].FullSpan;
                return new TextSpan(start.Start, stop.End - start.Start);
            }
        }

        public Segment Segment { get; }

        public override Token Token { get; }

        public override void Accept(INodeVisitor visitor)
        {
            visitor.Visit(this);
            _node.Accept(visitor);
        }

        public override IList<int> Evaluate(Segment segment) => _node.Evaluate(segment);

        public override string ToString() => _node.ToString();
    }
}