using System;
using TQL.CronExpression.Parser.Nodes;
using TQL.CronExpression.TimelineEvaluator.Evaluators;

namespace TQL.CronExpression.TimelineEvaluator
{
    public class CronTimelineVisitor : CronNodeVisitorBase, IEvaluable<ICronFireTimeEvaluator>
    {
        private readonly DateTimeOffset _referenceTime;
        private bool _isVisited;

        public CronTimelineVisitor(DateTimeOffset referenceTime)
        {
            this._referenceTime = referenceTime;
        }

        public CronTimelineVisitor()
            : this(DateTimeOffset.UtcNow)
        {
        }

        public ICronFireTimeEvaluator Evaluator =>
            new CronForwardFireTimeEvaluator(
                this[Parser.Enums.Segment.Year],
                this[Parser.Enums.Segment.Month],
                this[Parser.Enums.Segment.DayOfMonth],
                this[Parser.Enums.Segment.DayOfWeek],
                this[Parser.Enums.Segment.Hours],
                this[Parser.Enums.Segment.Minutes],
                this[Parser.Enums.Segment.Seconds],
                Time
            );

        public override void Visit(EndOfFileNode node)
        {
            _isVisited = true;
        }
    }
}