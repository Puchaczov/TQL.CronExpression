using Cron.Parser.Visitors;

namespace Cron.Parser.Nodes
{
    public interface IVisitedNode
    {
        void Accept(INodeVisitor visitor);
    }
}
