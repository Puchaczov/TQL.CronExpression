using Cron.Parser.Enums;
using Cron.Parser.Tokens;
using Cron.Parser.Visitors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cron.Parser.Nodes
{
    public abstract class SyntaxNode : IVisitedOperator
    {
        public abstract void Accept(INodeVisitor visitor);
        public abstract SyntaxNode[] Items { get; }
        public abstract Token Token { get; }
        public abstract IList<int> Evaluate(Segment segment);
    }
}
