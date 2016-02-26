using Cron.Parser.Enums;
using Cron.Parser.Tokens;
using Cron.Parser.Visitors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cron.Parser.Syntax
{
    public class MaxRangeValueNode : SyntaxOperatorNode
    {
        private SyntaxOperatorNode value;

        public MaxRangeValueNode(SyntaxOperatorNode value)
        {
            this.value = value;
        }

        public override SyntaxNode[] Items
        {
            get
            {
                return new SyntaxNode[]
                {
                    value
                };
            }
        }

        public override Token Token
        {
            get
            {
                return value.Token;
            }
        }

        public override void Accept(INodeVisitor visitor)
        {
            visitor.Visit(this);
        }

        public override IList<int> Evaluate(Segment segment)
        {
            return value.Evaluate(segment);
        }
    }
}
