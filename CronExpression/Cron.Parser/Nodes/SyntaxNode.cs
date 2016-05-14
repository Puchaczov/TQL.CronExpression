using Cron.Parser.Enums;
using Cron.Parser.Tokens;
using Cron.Parser.Visitors;
using System.Collections.Generic;

namespace Cron.Parser.Nodes
{
    public abstract class SyntaxNode : IVisitedNode
    {
        /// <summary>
        /// Get child items of node.
        /// </summary>
        public abstract SyntaxNode[] Desecendants { get; }

        /// <summary>
        /// Get span for whole expression independently, how complex it is.
        /// </summary>
        public abstract TextSpan FullSpan { get; }

        /// <summary>
        /// Determine whetever node is leaf or not.
        /// </summary>
        public abstract bool IsLeaf { get; }

        /// <summary>
        /// Token assigned to node. Can be operator, numeric, etc.
        /// </summary>
        public abstract Token Token { get; }
        /// <summary>
        /// Visitor entry point.
        /// </summary>
        /// <param name="visitor"></param>
        public abstract void Accept(INodeVisitor visitor);

        /// <summary>
        /// Helps expand simplest cron cases for evaluation purposes.
        /// </summary>
        /// <param name="segment"></param>
        /// <returns></returns>
        public abstract IList<int> Evaluate(Segment segment);
    }
}
