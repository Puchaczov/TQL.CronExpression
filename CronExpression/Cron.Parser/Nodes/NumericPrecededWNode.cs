using Cron.Parser.Enums;
using Cron.Parser.Extensions;
using Cron.Parser.Tokens;
using Cron.Parser.Visitors;
using System.Collections.Generic;

namespace Cron.Parser.Nodes
{
    public class NumericPrecededWNode : NumberNode
    {
        public NumericPrecededWNode(Token token)
            : base(token)
        { }

        public override void Accept(INodeVisitor visitor)
        {
            visitor.Visit(this);
        }

        public override IList<int> Evaluate(Segment segment) => ListExtension.Empty();

        public override string ToString() => Token.Value + "W";
    }
}
