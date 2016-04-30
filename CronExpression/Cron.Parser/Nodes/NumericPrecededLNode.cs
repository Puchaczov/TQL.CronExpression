using Cron.Parser.Tokens;
using System.Collections.Generic;
using Cron.Parser.Visitors;
using Cron.Parser.Enums;
using Cron.Parser.Extensions;

namespace Cron.Parser.Nodes
{
    public class NumericPrecededLNode : NumberNode
    {
        public NumericPrecededLNode(Token token)
            : base(token)
        { }

        public override void Accept(INodeVisitor visitor)
        {
            visitor.Visit(this);
        }

        public override IList<int> Evaluate(Segment segment) => ListExtension.Empty();

        public override string ToString() => Token.Value + "L";
    }
}
