namespace TQL.CronExpression.Parser.Helpers
{
    public struct NodeTraverseResult
    {
        public bool BreakNow { get; }
        public bool GoDepper { get; }

        public NodeTraverseResult(bool goDeeper, bool breakNow)
        {
            BreakNow = breakNow;
            GoDepper = goDeeper;
        }
    }
}