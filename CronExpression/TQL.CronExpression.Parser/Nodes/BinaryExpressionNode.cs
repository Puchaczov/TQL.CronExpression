using TQL.Core.Tokens;
using TQL.CronExpression.Parser.Tokens;

namespace TQL.CronExpression.Parser.Nodes
{
    public abstract class BinaryExpressionNode : CronSyntaxNode
    {
        protected BinaryExpressionNode(Token token)
        {
            Token = token;
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

        public override bool IsLeaf => true;
        public abstract CronSyntaxNode Left { get; }
        public abstract CronSyntaxNode Right { get; }

        public override Token Token { get; }
    }
}