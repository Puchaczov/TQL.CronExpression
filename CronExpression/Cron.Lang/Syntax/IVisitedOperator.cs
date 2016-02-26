using Cron.Parser.Visitors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cron.Parser.Syntax
{
    public interface IVisitedOperator
    {
        void Accept(INodeVisitor visitor);
    }
}
