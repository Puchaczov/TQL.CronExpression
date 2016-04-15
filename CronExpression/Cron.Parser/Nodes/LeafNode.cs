using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cron.Parser.Tokens;

namespace Cron.Parser.Nodes
{
    public abstract class LeafNode : SyntaxNode
    {
        private readonly Token token;

        protected LeafNode(Token token)
            : base()
        {
            this.token = token;
        }

        public override TextSpan FullSpan
        {
            get
            {
                return Token.Span.Clone();
            }
        }

        public override bool IsLeaf
        {
            get
            {
                return true;
            }
        }

        public override Token Token
        {
            get
            {
                return token;
            }
        }
    }
}
