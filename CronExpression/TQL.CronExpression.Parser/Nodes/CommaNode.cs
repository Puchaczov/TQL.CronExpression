using System.Collections.Generic;
using System.Linq;
using TQL.Core.Tokens;
using TQL.CronExpression.Parser.Enums;
using TQL.CronExpression.Parser.Tokens;

namespace TQL.CronExpression.Parser.Nodes
{
    public class CommaNode : BinaryExpressionNode
    {
        private readonly CronSyntaxNode _left;
        private readonly CronSyntaxNode _right;

        public CommaNode(CronSyntaxNode left, CronSyntaxNode right, Token token)
            : base(token)
        {
            this._left = left;
            this._right = right;
        }

        public override CronSyntaxNode[] Desecendants
        {
            get
            {
                var commaItems = new List<CronSyntaxNode>();
                var current = _left;
                commaItems.Add(_right);
                while (current != null)
                {
                    var commaCurrent = current as CommaNode;
                    if (commaCurrent != null)
                    {
                        current = commaCurrent.Left;
                        commaItems.Add(commaCurrent.Right);
                    }
                    else
                    {
                        commaItems.Add(current);
                        current = null;
                    }
                }
                commaItems.Reverse();
                return commaItems.ToArray();
            }
        }

        public override TextSpan FullSpan
        {
            get
            {
                var items = Desecendants;
                var stop = items.Last().FullSpan.End;
                var start = items.First().FullSpan.Start;
                return new TextSpan(start, stop - start);
            }
        }

        public override CronSyntaxNode Left => _left;

        public override CronSyntaxNode Right => _right;

        public override void Accept(INodeVisitor visitor)
        {
            visitor.Visit(this);
            _left.Accept(visitor);
            _right.Accept(visitor);
        }

        public override IList<int> Evaluate(Segment segment)
        {
            var list = new List<int>();
            foreach (var item in Desecendants)
                list.AddRange(item.Evaluate(segment));
            return list;
        }

        public override string ToString() => _left + Token.Value + _right;
    }
}