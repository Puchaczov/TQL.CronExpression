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
    public class WordNode : LeafNode
    {
        private readonly Token token;

        public WordNode(Token token)
            : base(token)
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

        public override SyntaxNode[] Desecendants
        {
            get
            {
                return new SyntaxNode[0];
            }
        }

        public override string ToString()
        {
            return Token.Value;
        }
    }
}
