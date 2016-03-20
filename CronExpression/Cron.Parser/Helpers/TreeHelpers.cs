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
            int pathIndex = 1;
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

        public static SyntaxNode FindBySpan(this RootComponentNode tree, TextSpan span)
        {
            List<SyntaxNode> candidates = new List<SyntaxNode>();
            TreeHelpers.Traverse(tree, (SyntaxNode node) => {
                if (span.IsEqual(node.FullSpan))
                {
                    candidates.Add(node);
                }
            });

            if(candidates.Count == 0)
            {
                return null;
            }

            return RemoveEndOfFileNodeAndReturnFirst(candidates);
        }

        public static SyntaxNode FindBySpan(this RootComponentNode tree, int caret)
        {
            List<SyntaxNode> candidates = new List<SyntaxNode>();
            var span = new TextSpan(caret, 1);
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

            return RemoveEndOfFileNodeAndReturnFirst(candidates);
        }

        public static void Traverse(this RootComponentNode tree, Action<SyntaxNode> fun)
        {
            List<SyntaxNode> nodesOnSameLevel = new List<SyntaxNode>();
            nodesOnSameLevel.Add(tree);
            fun(tree);
            while(nodesOnSameLevel.Count > 0)
            {
                var currentItem = 0;
                var child = nodesOnSameLevel[currentItem].Desecendants;
                for (int i = 0; i < child.Count(); ++i)
                {
                    fun(child[i]);
                    if(child[i].Desecendants.Count() > 0)
                    {
                        nodesOnSameLevel.Add(child[i]);
                    }
                }
                nodesOnSameLevel.RemoveAt(currentItem);
            }
        }

        private static SyntaxNode RemoveEndOfFileNodeAndReturnFirst(List<SyntaxNode> candidates)
        {
            int i = 0;
            for (; i < candidates.Count; ++i)
            {
                //Can't use it becouse it's zero-length.
                if (candidates[i] is EndOfFileNode)
                {
                    candidates.RemoveAt(i);
                    break;
                }
            }
            return candidates.FirstOrDefault();
        }
    }
}
