using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cron.Parser.Tokens;

namespace Cron.Parser.Nodes
{
    public abstract class UnaryExpressionNode : SyntaxNode
    {
        public virtual SyntaxNode Descendant => Desecendants[0];

        public override TextSpan FullSpan => Descendant.FullSpan.Clone();

        public override bool IsLeaf => false;
    }
}
