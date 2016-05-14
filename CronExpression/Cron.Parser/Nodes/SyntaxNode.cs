using Cron.Core.Syntax;
using Cron.Parser.Enums;
using Cron.Parser.Tokens;
using Cron.Parser.Visitors;
using System.Collections.Generic;

namespace Cron.Parser.Nodes
{
    public abstract class CronSyntaxNode : SyntaxNodeBase<INodeVisitor, TokenType>
    {
        /// <summary>
        /// Get child items of node.
        /// </summary>
        public abstract CronSyntaxNode[] Desecendants { get; }

        /// <summary>
        /// Token assigned to node. Can be operator, numeric, etc.
        /// </summary>
        public abstract Token Token { get; }

        /// <summary>
        /// Helps expand simplest cron cases for evaluation purposes.
        /// </summary>
        /// <param name="segment"></param>
        /// <returns></returns>
        public abstract IList<int> Evaluate(Segment segment);
    }
}
