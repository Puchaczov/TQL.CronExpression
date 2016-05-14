using Cron.Core.Tokens;
using Cron.Parser.Enums;
using Cron.Parser.Tokens;
using Cron.Parser.Visitors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cron.Parser.Nodes
{
    public class RootComponentNode : CronSyntaxNode
    {
        private readonly SegmentNode[] cronComponents;

        public RootComponentNode(SegmentNode[] cronComponents)
        {
            this.cronComponents = cronComponents;
        }

        public override CronSyntaxNode[] Desecendants => cronComponents;

        public SegmentNode[] Segments => cronComponents;

        public override TextSpan FullSpan
        {
            get
            {
                var start = Desecendants.First().FullSpan.Start;
                var stop = Desecendants.Last().FullSpan.End;
                return new TextSpan(start, stop - start);
            }
        }

        public override bool IsLeaf => false;

        public override Token Token => null;

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
