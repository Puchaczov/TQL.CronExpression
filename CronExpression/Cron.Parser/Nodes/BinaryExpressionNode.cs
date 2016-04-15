using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cron.Parser.Tokens;

namespace Cron.Parser.Nodes
{
    public abstract class BinaryExpressionNode : SyntaxNode
    {
        private readonly Token token;

        protected BinaryExpressionNode(Token token)
        {
            this.token = token;
        }

        public override TextSpan FullSpan
        {
            get
            {
                var start = Left.Token.Span.Start;
                var stop = Right.Token.Span.End;
                return new TextSpan(start, stop - start);
            }
        }

        public override bool IsLeaf
        {
            get
            {
                return true;
            }
        }
        public abstract SyntaxNode Left { get; }
        public abstract SyntaxNode Right { get; }

        public override Token Token
        {
            get
            {
                return token;
            }
        }
    }
}
