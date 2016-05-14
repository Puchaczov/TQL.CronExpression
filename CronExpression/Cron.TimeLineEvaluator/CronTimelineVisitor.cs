using System;
using Cron.Parser.Nodes;
using Cron.Extensions.TimelineEvaluator.Evaluators;

namespace Cron.Extensions.TimelineEvaluator
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
                return new CronFireTimeEvaluator(
                    this[Cron.Parser.Enums.Segment.Year],
                    this[Cron.Parser.Enums.Segment.Month],
                    this[Cron.Parser.Enums.Segment.DayOfMonth],
                    this[Cron.Parser.Enums.Segment.DayOfWeek],
                    this[Cron.Parser.Enums.Segment.Hours],
                    this[Cron.Parser.Enums.Segment.Minutes],
                    this[Cron.Parser.Enums.Segment.Seconds],
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
