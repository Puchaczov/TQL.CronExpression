using Cron.Parser.Enums;
using Cron.Parser.Tokens;
using Cron.Parser.Visitors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Cron.Parser.Nodes
{
    public class RootComponentNode : SyntaxNode
    {
        private SegmentNode[] cronComponents;

        public RootComponentNode(SegmentNode[] cronComponents)
        {
            this.cronComponents = cronComponents;
        }

        public override void Accept(INodeVisitor visitor)
        {
            visitor.Visit(this);
            foreach (var item in cronComponents)
            {
                item.Accept(visitor);
            }
        }

        public override IList<int> Evaluate(Segment segment)
        {
            throw new NotImplementedException();
        }

        public override SyntaxNode[] Desecendants
        {
            get
            {
                return cronComponents;
            }
        }

        public override Token Token
        {
            get
            {
                return null;
            }
        }

        public override bool IsLeaf
        {
            get
            {
                return false;
            }
        }

        public override TextSpan FullSpan
        {
            get
            {
                var start = Desecendants.First().FullSpan.Start;
                var stop = Desecendants.Last().FullSpan.End;
                return new TextSpan(start, stop - start);
            }
        }

        public override string ToString()
        {
            var stringifiedNodes = new StringBuilder();
            for (int i = 0, j = Desecendants.Count() - 2; i < j; ++i)
            {
                stringifiedNodes.Append(Desecendants[i].ToString());
                stringifiedNodes.Append(' ');
            }
            return stringifiedNodes.Append(Desecendants[Desecendants.Count() - 2].ToString()).ToString();
        }
    }
}
