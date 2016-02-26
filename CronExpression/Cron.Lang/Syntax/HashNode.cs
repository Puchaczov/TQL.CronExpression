using Cron.Parser.Enums;
using Cron.Parser.Extensions;
using Cron.Parser.Tokens;
using Cron.Parser.Visitors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cron.Parser.Syntax
{
    public class HashNode : SyntaxOperatorNode
    {
        private Token left;
        private Token right;

        public HashNode(Token left, Token right)
        {
            this.left = left;
            this.right = right;
        }

        public override void Accept(INodeVisitor visitor)
        {
            visitor.Visit(this);
        }

        public override IList<int> Evaluate(Segment segment)
        {
            return ListExtension.Empty();
        }

        public Token Left
        {
            get
            {
                return left;
            }
        }

        public Token Right
        {
            get
            {
                return right;
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

        public override Token Token
        {
            get
            {
                return new HashToken();
            }
        }
    }
}
