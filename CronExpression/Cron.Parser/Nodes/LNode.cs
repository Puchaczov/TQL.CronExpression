﻿using System.Collections.Generic;
using TQL.CronExpression.Parser.Enums;
using TQL.CronExpression.Parser.Extensions;
using TQL.CronExpression.Parser.Tokens;
using TQL.CronExpression.Parser.Visitors;

namespace TQL.CronExpression.Parser.Nodes
{
    public class LNode : NumberNode
    {
        public LNode(Token token)
            : base(token)
        { }

        public override void Accept(INodeVisitor visitor)
        {
            visitor.Visit(this);
        }

        public override IList<int> Evaluate(Segment segment) => ListExtension.Empty();

        public override string ToString() => "L";
    }
}
