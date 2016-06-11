using System;
using TQL.CronExpression.Extensions.TimelineEvaluator.Evaluators;
using TQL.CronExpression.Parser.Nodes;

namespace TQL.CronExpression.Extensions.TimelineEvaluator
{
    public class CronTimelineVisitor : CronNodeVisitorBase, IEvaluable<ICronFireTimeEvaluator>
    {
        private bool isVisited;
        private readonly DateTimeOffset referenceTime;

        public CronTimelineVisitor(DateTimeOffset referenceTime)
        {
            this.referenceTime = referenceTime;
        }

        public CronTimelineVisitor()
            : this(DateTimeOffset.UtcNow)
        { }

        public ICronFireTimeEvaluator Evaluator
        {
            get
            {
                if (!isVisited)
                {
                    return null;
                }
                if(e.Count > 0)
                {
                    return null;
                }
                return new CronForwardFireTimeEvaluator(
                    this[Parser.Enums.Segment.Year],
                    this[Parser.Enums.Segment.Month],
                    this[Parser.Enums.Segment.DayOfMonth],
                    this[Parser.Enums.Segment.DayOfWeek],
                    this[Parser.Enums.Segment.Hours],
                    this[Parser.Enums.Segment.Minutes],
                    this[Parser.Enums.Segment.Seconds],
                    time);
            }
        }

        public override void Visit(EndOfFileNode node)
        {
            base.Visit(node);
            isVisited = true;
        }
    }
}
