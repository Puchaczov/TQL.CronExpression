using Cron.Parser.Enums;
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
            for (; pathIndex < splitedPath.Count(); ++pathIndex)
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
            var candidates = new List<SyntaxNode>();
            TreeHelpers.Traverse(tree, (SyntaxNode node, SyntaxNode parent) =>
            {
                if (span.IsInside(node.FullSpan))
                {
                    candidates.Add(node);
                }
                return new NodeTraverseResult(true, false);
            });

            if (candidates.Count == 0)
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

        public static SegmentNode GetSegmentByCaret(this RootComponentNode tree, int caret)
        {
            foreach (var segment in tree.Desecendants)
            {
                if (caret >= segment.FullSpan.Start && caret <= segment.FullSpan.End)
                {
                    return (SegmentNode)segment;
                }
            }
            return null;
        }

        public static void Traverse(this RootComponentNode tree, Action<SyntaxNode> fun)
        {
            Traverse(tree, (obj, parent) => {
                fun?.Invoke(obj);
                return new NodeTraverseResult(true, false);
            });
        }

        public static void Traverse(this RootComponentNode tree, Func<SyntaxNode, SyntaxNode, NodeTraverseResult> fun)
        {
            Traverse((SyntaxNode)tree, fun);
        }

        public static SyntaxNode[] Siblings(this SyntaxNode startNode, SyntaxNode oneOfSiblings)
        {
            SyntaxNode[] siblings = null;

            Traverse(startNode, (SyntaxNode node, SyntaxNode parent) => {
                if(ReferenceEquals(node, oneOfSiblings))
                {
                    siblings = parent?.Desecendants.Where(f => !ReferenceEquals(f, oneOfSiblings)).ToArray();
                    return new NodeTraverseResult(false, true);
                }
                return new NodeTraverseResult(true, false);
            });

            return siblings;
        }

        public static SegmentNode FindSegment(this RootComponentNode root, Segment segment)
        {
            var segments = root.Segments;
            for(int i = 0, j = (int)segment; i < j; ++i)
            {
                if(segments[i].Segment == segment)
                {
                    return segments[i];
                }
            }
            return null;
        }

        public static void Traverse(this SyntaxNode node, Func<SyntaxNode, SyntaxNode, NodeTraverseResult> action)
        {
            if (node == null)
            {
                throw new ArgumentNullException(nameof(node));
            }

            if (action == null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            var nodesOnSameLevel = new List<SyntaxNode>();
            nodesOnSameLevel.Add(node);

            var result = action(node, null);
            if(!result.GoDepper || result.BreakNow)
            {
                return;
            }

            const int firstItem = 0;
            while (nodesOnSameLevel.Count > 0)
            {
                var parent = nodesOnSameLevel[firstItem];
                var child = parent.Desecendants;
                
                for (int i = 0; i < child.Count(); ++i)
                {
                    result = action(child[i], parent);
                    if(!result.GoDepper)
                    {
                        continue;
                    }
                    if(result.BreakNow)
                    {
                        return;
                    }
                    if (child[i].Desecendants.Count() > 0)
                    {
                        nodesOnSameLevel.Add(child[i]);
                    }
                }
                nodesOnSameLevel.RemoveAt(firstItem);
            }
        }
    }

    public struct NodeTraverseResult
    {
        public bool BreakNow { get; private set; }
        public bool GoDepper { get; private set; }

        public NodeTraverseResult(bool goDeeper, bool breakNow)
        {
            this.BreakNow = breakNow;
            this.GoDepper = goDeeper;
        }
    }
}
