using Cron.Parser.Tokens;
using System.Collections.Generic;
using Cron.Parser.Visitors;
using Cron.Parser.Extensions;
using Cron.Parser.Enums;

namespace Cron.Parser.Nodes
{
    public class LNode : NumberNode
    {
        public LNode(Token token)
            : base(token)
        { }

        public override void Accept(INodeVisitor visitor)
        {
            visitor.Visit(this);
        }

        public override IList<int> Evaluate(Segment segment) => ListExtension.Empty();

        public override string ToString() => "L";
    }
}
