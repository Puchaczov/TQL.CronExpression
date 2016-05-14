using Cron.Parser.Enums;
using Cron.Parser.Extensions;
using Cron.Parser.Tokens;
using System.Collections.Generic;
using Cron.Parser.Visitors;

namespace Cron.Parser.Nodes
{
    public class LWNode : LeafNode
    {
        public LWNode(Token token)
            : base(token)
        { }

        public override CronSyntaxNode[] Desecendants => new CronSyntaxNode[0];

        public override void Accept(INodeVisitor visitor)
        {
            visitor.Visit(this);
        }

        public override IList<int> Evaluate(Segment segment) => ListExtension.Empty();

        public override string ToString() => "LW";
    }
}
