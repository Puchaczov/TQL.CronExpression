using System;
using System.Collections.Generic;
using Cron.Parser.Tokens;
using Cron.Parser.Visitors;
using Cron.Parser.Enums;

namespace Cron.Parser.Syntax
{
    public class MinRangeValueNode : SyntaxOperatorNode
    {
        private SyntaxOperatorNode value;

        public MinRangeValueNode(SyntaxOperatorNode value)
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
