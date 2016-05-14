using Cron.Parser.Enums;
using Cron.Parser.Exceptions;
using Cron.Parser.Extensions;
using Cron.Parser.Tokens;
using Cron.Parser.Visitors;
using System.Collections.Generic;

namespace Cron.Parser.Nodes
{
    public class StarNode : LeafNode
    {
        private readonly Segment segment;

        public StarNode(Segment segment, Token token)
            : base(token)
        {
            this.segment = segment;
        }

        public override CronSyntaxNode[] Desecendants => new CronSyntaxNode[0];

        public Segment Segment => segment;

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
                    return ListExtension.Expand(1, 31, 1);
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
