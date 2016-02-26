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
    public class NumberNode : SyntaxOperatorNode, IValuableExpression
    {
        private Token token;

        public NumberNode(Token token)
        {
            this.token = token;
        }

        public override void Accept(INodeVisitor visitor)
        {
            visitor.Visit(this);
        }

        public override IList<int> Evaluate(Segment segment)
        {
            return new List<int> { int.Parse(token.Value) };
        }

        public string Value
        {
            get
            {
                return token.Value;
            }
        }

        public SyntaxOperatorNode Self
        {
            get
            {
                return this;
            }
        }

        public override Token Token
        {
            get
            {
                return token;
            }
        }

        public override SyntaxNode[] Items
        {
            get
            {
                return new SyntaxNode[] {
                };
            }
        }
    }
}
