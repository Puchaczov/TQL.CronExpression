using System;
using Cron.Parser.Syntax;
using Cron.Visitors.Evaluators;

namespace Cron.Visitors
{
    public class CronTimelineVisitor : CronNodeVisitorBase
    {
        private DateTimeOffset referenceTime;
        private bool isVisited = false;

        public ICronEvaluator Analyzer
        {
            get
            {
                if (!isVisited)
                {
                    throw new Exception("Nodes must be visited first");
                }
                return new CronEvaluator(
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

        public CronTimelineVisitor(DateTime referenceTime)
        {
            this.referenceTime = referenceTime;
        }

        public CronTimelineVisitor()
            : this(DateTime.UtcNow)
        { }

        public override void Visit(EndOfFileNode node)
        {
            base.Visit(node);
            isVisited = true;
        }
    }
}
