using System;
using System.Collections.Generic;
using System.Linq;
using TQL.CronExpression.Extensions.TimelineEvaluator.List;
using TQL.CronExpression.Extensions.TimelineEvaluator.Lists.ComputableLists;
using TQL.CronExpression.Parser.Enums;
using TQL.CronExpression.Parser.Exceptions;
using TQL.CronExpression.Parser.Extensions;
using TQL.CronExpression.Parser.Nodes;
using TQL.CronExpression.Parser.Utils;
using TQL.CronExpression.Visitors;

namespace TQL.CronExpression.Extensions.TimelineEvaluator
{
    public class CronNodeVisitorBase : CronRulesNodeVisitor
    {
        protected readonly Ref<DateTimeOffset> time;
        private Segment lastSegment;
        private readonly Dictionary<Segment, RoundRobinRangeVaryingList<int>> values;

        public CronNodeVisitorBase()
        {
            values = new Dictionary<Segment, RoundRobinRangeVaryingList<int>>();
            DateTimeOffset refTime = DateTimeOffset.UtcNow;
            time = new Ref<DateTimeOffset>(() => refTime, time => { refTime = time; });
        }

        public Dictionary<Segment, RoundRobinRangeVaryingList<int>> Result => values;

        public RoundRobinRangeVaryingList<int> this[Segment segment] => Result[segment];

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
                    var list = new EverydayOfWeekComputeList(time);
                    values[lastSegment].Add(list);
                    values[lastSegment].SetRange(0, list.Count - 1);
                    break;
            }
        }

        public override void Visit(SegmentNode node)
        {
            try
            {
                base.Visit(node);
                lastSegment = node.Segment;
                values.Add(lastSegment, new RoundRobinRangeVaryingList<int>());
                var evaluated = node.Evaluate(node.Segment);
                var persistent = new PersistentList<int>(evaluated.OrderBy(f => f).Distinct());
                values[lastSegment].Add(persistent);
                values[lastSegment].SetRange(0, persistent.Count - 1);
            }
            catch(EvaluationException ex)
            {
                criticalErrors.Add(ex);
            }
        }

        public override void Visit(RootComponentNode node)
        {
            base.Visit(node);
        }

        public override void Visit(RangeNode node)
        {
            base.Visit(node);

            var left = 0;
            var right = 0;
            var skipNumericEvaluation = false;
            switch (lastSegment)
            {
                case Segment.DayOfWeek:
                    left = CronWordHelper.DayOfWeek(node.Left.Token.Value).AsInt();
                    right = CronWordHelper.DayOfWeek(node.Right.Token.Value).AsInt();
                    foreach (var dayOfWeek in ListExtension.Expand(left, right, 1))
                    {
                        values[lastSegment].Add(new NthDayOfMonthComputeList(time, dayOfWeek.AsDayOfWeek()));
                    }
                    break;
                case Segment.Month:
                    left = CronWordHelper.Month(node.Left.Token.Value).AsInt();
                    right = CronWordHelper.Month(node.Right.Token.Value).AsInt();
                    skipNumericEvaluation = true;
                    goto default;
                default:
                    if (!skipNumericEvaluation)
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
                        new NthDayOfMonthComputeList(time, CronWordHelper.DayOfWeek(node.Token.Value))
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
            switch (lastSegment)
            {
                case Segment.DayOfWeek:
                    var list = new EverydayOfWeekComputeList(time);
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
            switch (lastSegment)
            {
                case Segment.DayOfMonth:
                    values[lastSegment].Add(new MonthBasedComputeList(time, new int[] { 0 }));
                    values[lastSegment].SetRange(0, values[lastSegment].Count);
                    break;
                case Segment.DayOfWeek:
                    values[lastSegment].Add(new LastDayOfWeekInMonthBasedOnCurrentMonthComputeList(time, new int[] { 0 }));
                    values[lastSegment].SetRange(0, values[lastSegment].Count);
                    break;
            }
        }

        public override void Visit(WNode node)
        {
            base.Visit(node);
            switch (lastSegment)
            {
                case Segment.DayOfMonth:
                    values[lastSegment].Add(new NearWeekdayComputeList(time, 1));
                    break;
            }
        }

        public override void Visit(HashNode node)
        {
            base.Visit(node);
            var dayOfWeek = CronWordHelper.DayOfWeek(node.Left.Token.Value);
            var nthOfMonth = int.Parse(node.Right.Token.Value);
            values[lastSegment].Add(new NthDayOfMonthLimitedByNumberOfWeekComputeList(time, dayOfWeek, nthOfMonth));
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
            switch (lastSegment)
            {
                case Segment.DayOfMonth:
                    values[lastSegment].Add(new MonthBasedComputeList(time, new int[] { int.Parse(node.Token.Value) }));
                    values[lastSegment].SetRange(0, count);
                    break;
                case Segment.DayOfWeek:
                    values[lastSegment].Add(new LastDayOfWeekInMonthBasedOnCurrentMonthComputeList(time, new int[] { int.Parse(node.Token.Value) }));
                    values[lastSegment].SetRange(0, count);
                    break;
            }
        }

        public override void Visit(NumericPrecededWNode node)
        {
            base.Visit(node);
            switch (lastSegment)
            {
                case Segment.DayOfMonth:
                    values[lastSegment].Add(new NearWeekdayComputeList(time, int.Parse(node.Token.Value)));
                    break;
            }
        }

        public override void Visit(LWNode node)
        {
            base.Visit(node);
            switch (lastSegment)
            {
                case Segment.DayOfMonth:
                    values[lastSegment].Add(new LastWeekdayOfMonthComputeList(time));
                    break;
            }
        }
    }
}
