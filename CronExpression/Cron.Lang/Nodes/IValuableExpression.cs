using Cron.Parser.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cron.Parser.Nodes
{
    public interface IValuableExpression
    {
        string Value { get; }
        SyntaxOperatorNode Self { get; }
        Token Token { get; }
    }
}
