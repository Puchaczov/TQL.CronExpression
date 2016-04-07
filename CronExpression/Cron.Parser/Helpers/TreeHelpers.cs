using Cron.Parser.Nodes;
using Cron.Parser.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cron.Parser.Helpers
{
    public static class TreeHelpers
    {
        public static SyntaxNode FindByPath(this RootComponentNode tree, string path)
        {
            var pathIndex = 1;
            SyntaxNode current = tree;
            var splitedPath = path.Split('>');
            for(;pathIndex < splitedPath.Count(); ++pathIndex)
            {
                var descIndex = Convert.ToInt16(splitedPath[pathIndex]);
                if (descIndex >= current.Desecendants.Count())
                {
                    throw new IndexOutOfRangeException();
                }
                current = current.Desecendants[descIndex];
            }
            return current;
        }

        public static SegmentNode GetSegmentByCaret(this RootComponentNode tree, int caret)
        {
            foreach(var segment in tree.Desecendants)
            {
                if(caret >= segment.FullSpan.Start && caret <= segment.FullSpan.End)
                {
                    return (SegmentNode)segment;
                }
            }
            return null;
        }

        public static SyntaxNode FindBySpan(this RootComponentNode tree, TextSpan span)
        {
            var candidates = new List<SyntaxNode>();
            TreeHelpers.Traverse(tree, (SyntaxNode node) => {
                if (span.IsInside(node.FullSpan))
                {
                    candidates.Add(node);
                }
            });

            if(candidates.Count == 0)
            {
                return null;
            }

            candidates = candidates.OrderBy(f => f.FullSpan.Length).ToList();

            return candidates.FirstOrDefault();
        }

        public static SyntaxNode FindBySpan(this RootComponentNode tree, int caret)
        {
            return FindBySpan(tree, new TextSpan(caret, 1));
        }

        public static void Traverse(this RootComponentNode tree, Action<SyntaxNode> fun)
        {
            Traverse((SyntaxNode)tree, fun);
        }

        public static void Traverse(this SyntaxNode node, Action<SyntaxNode> fun)
        {
            if(node == null)
            {
                throw new ArgumentNullException(nameof(node));
            }

            if(fun == null)
            {
                throw new ArgumentNullException(nameof(fun));
            }

            var nodesOnSameLevel = new List<SyntaxNode>();
            nodesOnSameLevel.Add(node);

            fun(node);

            const int firstItem = 0;
            while (nodesOnSameLevel.Count > 0)
            {
                var child = nodesOnSameLevel[firstItem].Desecendants;
                for (int i = 0; i < child.Count(); ++i)
                {
                    fun(child[i]);
                    if (child[i].Desecendants.Count() > 0)
                    {
                        nodesOnSameLevel.Add(child[i]);
                    }
                }
                nodesOnSameLevel.RemoveAt(firstItem);
            }
        }
    }
}
