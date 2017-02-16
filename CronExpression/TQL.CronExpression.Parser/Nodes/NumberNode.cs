using System.Collections.Generic;
using TQL.CronExpression.Parser.Enums;
using TQL.CronExpression.Parser.Tokens;

namespace TQL.CronExpression.Parser.Nodes
{
    public class NumberNode : LeafNode
    {
        public NumberNode(Token token)
            : base(token)
        {
        }

        public override CronSyntaxNode[] Desecendants => new CronSyntaxNode[0];

        public override void Accept(INodeVisitor visitor)
        {
            visitor.Visit(this);
        }

        public override IList<int> Evaluate(Segment segment) => new List<int> {int.Parse(Token.Value)};

        public override string ToString() => Token.Value;
    }
}