using Cron.Parser.Enums;
using Cron.Parser.Nodes;
using System;
using System.Collections.Generic;
using System.Linq;
using TQL.Core.Tokens;

namespace Cron.Parser.Helpers
{
    public static class TreeHelpers
    {
        public static CronSyntaxNode FindByPath(this RootComponentNode tree, string path)
        {
            var pathIndex = 1;
            CronSyntaxNode current = tree;
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

        public static CronSyntaxNode FindBySpan(this RootComponentNode tree, TextSpan span)
        {
            var candidates = new List<CronSyntaxNode>();
            TreeHelpers.Traverse(tree, (CronSyntaxNode node, CronSyntaxNode parent) =>
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

        public static CronSyntaxNode FindBySpan(this RootComponentNode tree, int caret) => FindBySpan(tree, new TextSpan(caret, 1));

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

        public static void Traverse(this RootComponentNode tree, Action<CronSyntaxNode> fun)
        {
            Traverse(tree, (obj, parent) => {
                fun?.Invoke(obj);
                return new NodeTraverseResult(true, false);
            });
        }

        public static void Traverse(this RootComponentNode tree, Func<CronSyntaxNode, CronSyntaxNode, NodeTraverseResult> fun)
        {
            Traverse((CronSyntaxNode)tree, fun);
        }

        public static CronSyntaxNode[] Siblings(this CronSyntaxNode startNode, CronSyntaxNode oneOfSiblings)
        {
            CronSyntaxNode[] siblings = null;

            Traverse(startNode, (CronSyntaxNode node, CronSyntaxNode parent) => {
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

        public static void Traverse(this CronSyntaxNode node, Func<CronSyntaxNode, CronSyntaxNode, NodeTraverseResult> action)
        {
            if (node == null)
            {
                throw new ArgumentNullException(nameof(node));
            }

            if (action == null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            var nodesOnSameLevel = new List<CronSyntaxNode>();
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
        public bool BreakNow { get; }
        public bool GoDepper { get; }

        public NodeTraverseResult(bool goDeeper, bool breakNow)
        {
            this.BreakNow = breakNow;
            this.GoDepper = goDeeper;
        }
    }
}
