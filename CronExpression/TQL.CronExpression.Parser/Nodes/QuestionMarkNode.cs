﻿using System.Collections.Generic;
using TQL.CronExpression.Parser.Enums;
using TQL.CronExpression.Parser.Exceptions;
using TQL.CronExpression.Parser.Extensions;
using TQL.CronExpression.Parser.Tokens;

namespace TQL.CronExpression.Parser.Nodes
{
    public class QuestionMarkNode : LeafNode
    {
        public QuestionMarkNode(Token token)
            : base(token)
        {
        }

        public override CronSyntaxNode[] Desecendants => new CronSyntaxNode[0];

        public override void Accept(INodeVisitor visitor)
        {
            visitor.Visit(this);
        }

        public override IList<int> Evaluate(Segment segment)
        {
            switch (segment)
            {
                case Segment.Seconds:
                    return ListExtension.Expand(0, 59, 1);
                case Segment.Minutes:
                    return ListExtension.Expand(0, 59, 1);
                case Segment.Hours:
                    return ListExtension.Expand(0, 23, 1);
                case Segment.DayOfWeek:
                    return ListExtension.Empty();
                case Segment.DayOfMonth:
                    return ListExtension.Expand(1, 32, 1);
                case Segment.Month:
                    return ListExtension.Expand(1, 12, 1);
                case Segment.Year:
                    return ListExtension.Expand(1970, 3000, 1);
                default:
                    throw new UnknownSegmentException(0);
            }
        }

        public override string ToString() => Token.Value;
    }
}