using Cron.Parser.Enums;
using Cron.Parser.Extensions;
using Cron.Parser.List;
using Cron.Parser.Nodes;
using Cron.Parser.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Cron.Visitors
{
    public class CronNodeVisitorBase : CronRulesNodeVisitor
    {
        private Dictionary<Segment, RoundRobinRangeVaryingList<int>> values;
        private Segment lastSegment;
        protected Ref<DateTimeOffset> time;

        public Dictionary<Segment, RoundRobinRangeVaryingList<int>> Result
        {
            get
            {
                return values;
            }
        }

        public RoundRobinRangeVaryingList<int> this[Segment segment]
        {
            get
            {
                return Result[segment];
            }
        }

        public CronNodeVisitorBase()
        {
            values = new Dictionary<Segment, RoundRobinRangeVaryingList<int>>();
            DateTimeOffset refTime = DateTime.Now;
            time = new Ref<DateTimeOffset>(() => refTime, time => { refTime = time; });
        }

        public override void Visit(SyntaxOperatorNode node)
        {
            base.Visit(node);
            node.Accept(this);
        }

        public override void Visit(CommaNode node)
        {
            base.Visit(node);
        }

        public override void Visit(StarNode node)
        {
            base.Visit(node);
            switch (lastSegment)
            {
                case Segment.DayOfWeek:
                    var list = new EveryDayOfWeekAllowedList(time);
                    values[lastSegment].Add(list);
                    values[lastSegment].SetRange(0, list.Count - 1);
                    break;
            }
        }

        public override void Visit(SegmentNode node)
        {
            base.Visit(node);
            lastSegment = node.Segment;
            values.Add(lastSegment, new RoundRobinRangeVaryingList<int>());
            var evaluated = node.Evaluate(node.Segment);
            var persistent = new PersistentList<int>(evaluated.Distinct().OrderBy(f => f).ToList());
            values[lastSegment].Add(persistent);
            values[lastSegment].SetRange(0, persistent.Count - 1);
        }

        public override void Visit(RootComponentNode node)
        {
            base.Visit(node);
        }

        public override void Visit(RangeNode node)
        {
            base.Visit(node);

            int left = 0;
            int right = 0;
            bool skipNumericEvaluation = false;
            switch(lastSegment)
            {
                case Segment.DayOfWeek:
                    left = CronWordHelper.DayOfWeek(node.Left.Token.Value).AsInt();
                    right = CronWordHelper.DayOfWeek(node.Right.Token.Value).AsInt();
                    foreach (var dayOfWeek in ListExtension.Expand(left, right, 1))
                    {
                        values[lastSegment].Add(new NthDayOfMonthList(time, dayOfWeek.AsDayOfWeek()));
                    }
                    break;
                case Segment.Month:
                    left = CronWordHelper.Month(node.Left.Token.Value).AsInt();
                    right = CronWordHelper.Month(node.Right.Token.Value).AsInt();
                    skipNumericEvaluation = true;
                    goto default;
                default:
                    if(!skipNumericEvaluation)
                    {
                        left = node.Left.Evaluate(lastSegment).First();
                        right = node.Right.Evaluate(lastSegment).First();
                    }
                    var lastCount = values[lastSegment].Count;
                    var expandedSegmentValues = ListExtension.Expand(left, right, 1);
                    values[lastSegment].Add(
                        new PersistentList<int>(expandedSegmentValues));
                    values[lastSegment].SetRange(0, lastCount + expandedSegmentValues.Count);
                    break;
            }
        }

        public override void Visit(WordNode node)
        {
            base.Visit(node);

            switch (lastSegment)
            {
                case Segment.DayOfWeek:
                    values[lastSegment].Add(
                        new NthDayOfMonthList(time, CronWordHelper.DayOfWeek(node.Token.Value))
                    );
                    break;
                case Segment.Month:
                    values[lastSegment].Add(
                            new PersistentList<int>(new List<int> { CronWordHelper.Month(node.Token.Value).AsInt() })
                        );
                    break;
                default:
                    throw new Exception("Bad segment");
            }
        }

        public override void Visit(NumberNode node)
        {
            base.Visit(node);
        }

        public override void Visit(QuestionMarkNode node)
        {
            base.Visit(node);
            switch(lastSegment)
            {
                case Segment.DayOfWeek:
                    var list = new EveryDayOfWeekAllowedList(time);
                    values[lastSegment].Add(list);
                    values[lastSegment].SetRange(0, list.Count - 1);
                    break;
            }
        }

        public override void Visit(IncrementByNode node)
        {
            base.Visit(node);
        }

        public override void Visit(LNode node)
        {
            base.Visit(node);
            switch(lastSegment)
            {
                case Segment.DayOfMonth:
                    values[lastSegment].Add(new MonthBasedComputedList(time, new int[] { 0 }));
                    values[lastSegment].SetRange(0, values[lastSegment].Count);
                    break;
                case Segment.DayOfWeek:
                    values[lastSegment].Add(new LastDayOfWeekInMonthBasedOnCurrentMonthComputedList(time, new int[] { int.Parse(node.Value) }));
                    values[lastSegment].SetRange(0, values[lastSegment].Count);
                    break;
            }
        }

        public override void Visit(WNode node)
        {
            base.Visit(node);
        }

        public override void Visit(HashNode node)
        {
            base.Visit(node);
            var dayOfWeek = CronWordHelper.DayOfWeek(node.Left.Value);
            var nthOfMonth = int.Parse(node.Right.Value);
            values[lastSegment].Add(new NthDayOfMonthLimitedByNumberOfWeekList(time, dayOfWeek, nthOfMonth));
            values[lastSegment].SetRange(0, values[lastSegment].Count);
        }

        public override void Visit(EndOfFileNode node)
        {
            base.Visit(node);
        }

        public override void Visit(NumericPrecededLNode node)
        {
            base.Visit(node);
            var count = values[lastSegment].Count;
            switch(lastSegment)
            {
                case Segment.DayOfMonth:
                    values[lastSegment].Add(new MonthBasedComputedList(time, new int[] { int.Parse(node.Value) }));
                    values[lastSegment].SetRange(0, count);
                    break;
                case Segment.DayOfWeek:
                    values[lastSegment].Add(new LastDayOfWeekInMonthBasedOnCurrentMonthComputedList(time, new int[] { int.Parse(node.Value) }));
                    values[lastSegment].SetRange(0, count);
                    break;
            }
        }

        public override void Visit(NumericPrecededWNode node)
        {
            base.Visit(node);
        }
    }
}
