using System.Collections.Generic;
using TQL.CronExpression.Parser.Enums;
using TQL.CronExpression.Parser.Extensions;
using TQL.CronExpression.Parser.Tokens;
using TQL.CronExpression.Parser.Visitors;

namespace TQL.CronExpression.Parser.Nodes
{
    public class WNode : NumberNode
    {
        private WToken token;
        public WNode(WToken token)
            : base(token)
        {
            this.token = token;
        }

        public override void Accept(INodeVisitor visitor)
        {
            visitor.Visit(this);
        }

        public override IList<int> Evaluate(Segment segment) => ListExtension.Empty();

        public override string ToString() => this.token.Number != 1 ? (base.Token as WToken).Value + "W" : "W";
    }
}
