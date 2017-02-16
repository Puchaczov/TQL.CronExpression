using System.Collections.Generic;
using TQL.CronExpression.Parser.Enums;
using TQL.CronExpression.Parser.Exceptions;
using TQL.CronExpression.Parser.Tokens;

namespace TQL.CronExpression.Parser.Nodes
{
    public class MissingNode : LeafNode
    {
        public MissingNode(Token token)
            : base(token)
        {
        }

        public override CronSyntaxNode[] Desecendants => new CronSyntaxNode[0];

        public override void Accept(INodeVisitor visitor)
        {
            visitor.Visit(this);
        }

        public override IList<int> Evaluate(Segment segment)
        {
            throw new EvaluationException(segment, "Cannot evaluate missing node.");
        }

        public override string ToString() => Token.Value;
    }
}