using System.Collections.Generic;
using TQL.Core.Syntax;
using TQL.CronExpression.Parser.Enums;
using TQL.CronExpression.Parser.Tokens;

namespace TQL.CronExpression.Parser.Nodes
{
    public abstract class CronSyntaxNode : SyntaxNodeBase<INodeVisitor, TokenType>
    {
        /// <summary>
        ///     Get child items of node.
        /// </summary>
        public abstract CronSyntaxNode[] Desecendants { get; }

        /// <summary>
        ///     Token assigned to node. Can be operator, numeric, etc.
        /// </summary>
        public abstract Token Token { get; }

        /// <summary>
        ///     Helps expand simplest cron cases for evaluation purposes.
        /// </summary>
        /// <param name="segment"></param>
        /// <returns></returns>
        public abstract IList<int> Evaluate(Segment segment);
    }
}