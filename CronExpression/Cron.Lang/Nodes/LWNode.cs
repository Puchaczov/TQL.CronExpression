using Cron.Parser.Enums;
using Cron.Parser.Extensions;
using Cron.Parser.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cron.Parser.Nodes
{
    public class LWNode : NumberNode
    {
        public LWNode(Token token)
            : base(token)
        { }

        public override IList<int> Evaluate(Segment segment)
        {
            return ListExtension.Empty();
        }

        public override string ToString()
        {
            return "LW";
        }
    }

    public class NumericPreceededLWNode : LWNode
    {
        public NumericPreceededLWNode(Token token)
            : base(token)
        { }

        public override string ToString()
        {
            return Token.Value + "LW";
        }
    }
}
