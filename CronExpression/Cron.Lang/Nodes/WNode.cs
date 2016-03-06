using Cron.Parser.Tokens;
using Cron.Parser.Visitors;
using System.Collections.Generic;
using Cron.Parser.Enums;
using Cron.Parser.Extensions;

namespace Cron.Parser.Nodes
{
    public class WNode : NumberNode
    {
        public WNode(Token token)
            : base(token)
        { }

        public override void Accept(INodeVisitor visitor)
        {
            visitor.Visit(this);
        }

        public override string ToString()
        {
            return "W";
        }

        public override IList<int> Evaluate(Segment segment)
        {
            return ListExtension.Empty();
        }
    }
}
