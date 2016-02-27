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
    public class RootComponentNode : SyntaxOperatorNode
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

        public override SyntaxNode[] Items
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
                throw new NotImplementedException();
            }
        }

        public override string ToString()
        {
            StringBuilder stringifiedNodes = new StringBuilder();
            for(int i = 0, j = Items.Count() - 2; i < j; ++i)
            {
                stringifiedNodes.Append(Items[i].ToString());
                stringifiedNodes.Append(' ');
            }
            return stringifiedNodes.Append(Items[Items.Count() - 2].ToString()).ToString();
        }
    }
}
