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
        public abstract SyntaxNode Descendant { get; }

        public override bool IsLeaf
        {
            get
            {
                return false;
            }
        }

        public override TextSpan FullSpan
        {
            get
            {
                return Descendant.FullSpan.Clone();
            }
        }
    }
}
