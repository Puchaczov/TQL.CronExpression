using System.Collections.Generic;
using TQL.CronExpression.Parser.Enums;
using TQL.CronExpression.Parser.Extensions;
using TQL.CronExpression.Parser.Tokens;

namespace TQL.CronExpression.Parser.Nodes
{
    public class HashNode : BinaryExpressionNode
    {
        private readonly CronSyntaxNode _left;
        private readonly CronSyntaxNode _right;

        public HashNode(CronSyntaxNode left, CronSyntaxNode right, Token token)
            : base(token)
        {
            this._left = left;
            this._right = right;
        }

        public override CronSyntaxNode[] Desecendants => new[]
        {
            _left,
            _right
        };

        public override CronSyntaxNode Left => _left;

        public override CronSyntaxNode Right => _right;

        public override void Accept(INodeVisitor visitor)
        {
            visitor.Visit(this);
        }

        public override IList<int> Evaluate(Segment segment) => ListExtension.Empty();

        public override string ToString() => Left + Token.Value + Right;
    }
}