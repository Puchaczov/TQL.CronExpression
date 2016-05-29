using Cron.Parser.Tokens;
using Cron.Parser.Visitors;
using TQL.Core.Tokens;

namespace Cron.Parser.Nodes
{
    public class EndOfFileNode : SegmentNode
    {
        public EndOfFileNode(Token token)
            : base(null, 0, token)
        { }

        public override CronSyntaxNode[] Desecendants => new CronSyntaxNode[0];

        public override TextSpan FullSpan => Token.Span;

        public override void Accept(INodeVisitor visitor)
        {
            visitor.Visit(this);
        }

        public override string ToString() => string.Empty;
    }
}