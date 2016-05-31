using TQL.CronExpression.Parser.Visitors;

namespace TQL.CronExpression.Parser.Nodes
{
    public interface IVisitedNode
    {
        void Accept(INodeVisitor visitor);
    }
}
