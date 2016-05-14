using Cron.Core.Tokens;
using System;

namespace Cron.Core.Syntax
{
    public abstract class SyntaxNodeBase<TVisitor, TTokenType> where TTokenType : struct, IComparable, IFormattable
    {
        /// <summary>
        /// Get child items of node.
        /// </summary>
        public abstract SyntaxNodeBase<TVisitor, TTokenType>[] Desecendants { get; }

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
        public abstract GenericToken<TTokenType> Token { get; }

        /// <summary>
        /// Visitor entry point.
        /// </summary>
        /// <param name="visitor"></param>
        public abstract void Accept(TVisitor visitor);
    }
}
