namespace TQL.CronExpression.Parser.Nodes
{
    public interface IVisitedNode
    {
        void Accept(INodeVisitor visitor);
    }
}
