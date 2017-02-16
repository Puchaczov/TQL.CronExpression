using System;
using System.Collections.Generic;
using System.Linq;
using TQL.CronExpression.Parser;
using TQL.CronExpression.TimelineEvaluator.Lists.ComputableLists;
using TQL.CronExpression.Parser.Enums;
using TQL.CronExpression.Parser.Exceptions;
using TQL.CronExpression.Parser.Extensions;
using TQL.CronExpression.Parser.Nodes;
using TQL.CronExpression.Parser.Utils;
using TQL.CronExpression.Parser.Tokens;
using TQL.CronExpression.TimelineEvaluator.Lists;

namespace TQL.CronExpression.TimelineEvaluator
{
    public class CronNodeVisitorBase : INodeVisitor
    {
        private readonly Dictionary<Segment, RoundRobinRangeVaryingList<int>> values;
        protected readonly Ref<DateTimeOffset> time;

        private Segment lastSegment;

        public CronNodeVisitorBase()
        {
            values = new Dictionary<Segment, RoundRobinRangeVaryingList<int>>();
            DateTimeOffset refTime = DateTimeOffset.UtcNow;
            time = new Ref<DateTimeOffset>(() => refTime, time => { refTime = time; });
        }

        public Dictionary<Segment, RoundRobinRangeVaryingList<int>> Result => values;

        public RoundRobinRangeVaryingList<int> this[Segment segment] => Result[segment];

        public void Visit(CommaNode node) { }

        public void Visit(StarNode node)
        {
            switch (lastSegment)
            {
                case Segment.DayOfWeek:
                    var list = new EverydayOfWeekComputeList(time);
                    values[lastSegment].Add(list);
                    values[lastSegment].SetRange(0, list.Count - 1);
                    break;
            }
        }

        public void Visit(SegmentNode node)
        {
            try
            {
                lastSegment = node.Segment;
                values.Add(lastSegment, new RoundRobinRangeVaryingList<int>());
                var evaluated = node.Evaluate(node.Segment);
                var persistent = new PersistentList<int>(evaluated.OrderBy(f => f).Distinct());
                values[lastSegment].Add(persistent);
                values[lastSegment].SetRange(0, persistent.Count - 1);
            }
            catch(EvaluationException)
            {
                throw;
            }
        }

        public void Visit(RootComponentNode node) { }

        public void Visit(RangeNode node)
        {
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

        public void Visit(WordNode node)
        {
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
            }
        }

        public void Visit(NumberNode node){ }

        public void Visit(QuestionMarkNode node)
        {
            switch (lastSegment)
            {
                case Segment.DayOfWeek:
                    var list = new EverydayOfWeekComputeList(time);
                    values[lastSegment].Add(list);
                    values[lastSegment].SetRange(0, list.Count - 1);
                    break;
            }
        }

        public void Visit(IncrementByNode node){}

        public void Visit(LNode node)
        {
            switch (lastSegment)
            {
                case Segment.DayOfMonth:
                    var token = node.Token as LToken;
                    //Default value of the token is 1 but L in this segment can be placed as L without 1 (eg.1L). But compute list operates on values from 0 to n
                    //TO DO: is it correct?????? 
                    var number = int.Parse(node.Token.Value);
                    if(number == 1)
                    {
                        number = 0;
                    }
                    values[lastSegment].Add(new MonthBasedComputeList(time, new int[] { number }));
                    values[lastSegment].SetRange(0, values[lastSegment].Count);
                    break;
                case Segment.DayOfWeek:
                    values[lastSegment].Add(new LastDayOfWeekInMonthBasedOnCurrentMonthComputeList(time, new int[] { int.Parse(node.Token.Value) }));
                    values[lastSegment].SetRange(0, values[lastSegment].Count);
                    break;
            }
        }

        public void Visit(WNode node)
        {
            switch (lastSegment)
            {
                case Segment.DayOfMonth:
                    values[lastSegment].Add(new NearWeekdayComputeList(time, int.Parse(node.Token.Value)));
                    break;
            }
        }

        public void Visit(HashNode node)
        {
            var dayOfWeek = CronWordHelper.DayOfWeek(node.Left.Token.Value);
            var nthOfMonth = int.Parse(node.Right.Token.Value);
            values[lastSegment].Add(new NthDayOfMonthLimitedByNumberOfWeekComputeList(time, dayOfWeek, nthOfMonth));
            values[lastSegment].SetRange(0, values[lastSegment].Count);
        }

        public virtual void Visit(EndOfFileNode node)
        { }

        public void Visit(NumericPrecededLNode node)
        {
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

        public void Visit(NumericPrecededWNode node)
        {
            switch (lastSegment)
            {
                case Segment.DayOfMonth:
                    values[lastSegment].Add(new NearWeekdayComputeList(time, int.Parse(node.Token.Value)));
                    break;
            }
        }

        public void Visit(LWNode node)
        {
            switch (lastSegment)
            {
                case Segment.DayOfMonth:
                    values[lastSegment].Add(new LastWeekdayOfMonthComputeList(time));
                    break;
            }
        }

        public void Visit(MissingNode node)
        { }
    }
}
