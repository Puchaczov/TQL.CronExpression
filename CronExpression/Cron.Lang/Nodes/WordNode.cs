using Cron.Parser.Tokens;
using Cron.Parser.Visitors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cron.Parser.Enums;
using Cron.Parser.Extensions;

namespace Cron.Parser.Nodes
{
    public class WordNode : SyntaxOperatorNode, IValuableExpression
    {
        private Token token;

        public WordNode(Token token)
        {
            this.token = token;
        }

        public override void Accept(INodeVisitor visitor)
        {
            visitor.Visit(this);
        }

        public override IList<int> Evaluate(Segment segment)
        {
            return ListExtension.Empty();
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

        public override string ToString()
        {
            return Token.Value;
        }
    }
}
