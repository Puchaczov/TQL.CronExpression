using System.Collections.Generic;
using TQL.CronExpression.Parser.Enums;
using TQL.CronExpression.Parser.Extensions;
using TQL.CronExpression.Parser.Tokens;

namespace TQL.CronExpression.Parser.Nodes
{
    public class LwNode : LeafNode
    {
        private readonly LwToken _token;

        public LwNode(LwToken token)
            : base(token)
        {
            this._token = token;
        }

        public override CronSyntaxNode[] Desecendants => new CronSyntaxNode[0];

        public override void Accept(INodeVisitor visitor)
        {
            visitor.Visit(this);
        }

        public override IList<int> Evaluate(Segment segment) => ListExtension.Empty();

        public override string ToString() => _token.Number != 1 ? (Token as LToken).Value + "LW" : "LW";
    }
}