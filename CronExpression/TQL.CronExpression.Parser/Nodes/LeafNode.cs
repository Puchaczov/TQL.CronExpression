using TQL.Core.Tokens;
using TQL.CronExpression.Parser.Tokens;

namespace TQL.CronExpression.Parser.Nodes
{
    public abstract class LeafNode : CronSyntaxNode
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
