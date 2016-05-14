using Cron.Core.Tokens;
using Cron.Parser.Tokens;

namespace Cron.Parser.Nodes
{
    public abstract class UnaryExpressionNode : CronSyntaxNode
    {
        public virtual CronSyntaxNode Descendant => Desecendants[0];

        public override TextSpan FullSpan => Descendant.FullSpan.Clone();

        public override bool IsLeaf => false;
    }
}
