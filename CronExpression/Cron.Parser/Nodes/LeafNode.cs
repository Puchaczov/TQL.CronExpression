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

        public override TextSpan FullSpan => Token.Span.Clone();

        public override bool IsLeaf => true;

        public override Token Token => token;
    }
}
