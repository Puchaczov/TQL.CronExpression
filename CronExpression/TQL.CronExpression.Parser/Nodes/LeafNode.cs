using TQL.Core.Tokens;
using TQL.CronExpression.Parser.Tokens;

namespace TQL.CronExpression.Parser.Nodes
{
    public abstract class LeafNode : CronSyntaxNode
    {
        protected LeafNode(Token token)
        {
            Token = token;
        }

        public override TextSpan FullSpan => Token.Span.Clone();

        public override bool IsLeaf => true;

        public override Token Token { get; }
    }
}