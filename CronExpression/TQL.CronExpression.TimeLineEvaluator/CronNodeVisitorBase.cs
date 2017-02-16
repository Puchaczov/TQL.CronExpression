using System;
using System.Collections.Generic;
using System.Linq;
using TQL.CronExpression.Parser;
using TQL.CronExpression.Parser.Enums;
using TQL.CronExpression.Parser.Exceptions;
using TQL.CronExpression.Parser.Extensions;
using TQL.CronExpression.Parser.Nodes;
using TQL.CronExpression.Parser.Tokens;
using TQL.CronExpression.Parser.Utils;
using TQL.CronExpression.TimelineEvaluator.Lists;
using TQL.CronExpression.TimelineEvaluator.Lists.ComputableLists;

namespace TQL.CronExpression.TimelineEvaluator
{
    public class CronNodeVisitorBase : INodeVisitor
    {
        protected readonly Ref<DateTimeOffset> Time;

        private Segment _lastSegment;

        public CronNodeVisitorBase()
        {
            Result = new Dictionary<Segment, RoundRobinRangeVaryingList<int>>();
            var refTime = DateTimeOffset.UtcNow;
            Time = new Ref<DateTimeOffset>(() => refTime, time => { refTime = time; });
        }

        public Dictionary<Segment, RoundRobinRangeVaryingList<int>> Result { get; }

        public RoundRobinRangeVaryingList<int> this[Segment segment] => Result[segment];

        public void Visit(CommaNode node)
        {
        }

        public void Visit(StarNode node)
        {
            switch (_lastSegment)
            {
                case Segment.DayOfWeek:
                    var list = new EverydayOfWeekComputeList(Time);
                    Result[_lastSegment].Add(list);
                    Result[_lastSegment].SetRange(0, list.Count - 1);
                    break;
            }
        }

        public void Visit(SegmentNode node)
        {
            try
            {
                _lastSegment = node.Segment;
                Result.Add(_lastSegment, new RoundRobinRangeVaryingList<int>());
                var evaluated = node.Evaluate(node.Segment);
                var persistent = new PersistentList<int>(evaluated.OrderBy(f => f).Distinct());
                Result[_lastSegment].Add(persistent);
                Result[_lastSegment].SetRange(0, persistent.Count - 1);
            }
            catch (EvaluationException)
            {
                throw;
            }
        }

        public void Visit(RootComponentNode node)
        {
        }

        public void Visit(RangeNode node)
        {
            var left = 0;
            var right = 0;
            var skipNumericEvaluation = false;
            switch (_lastSegment)
            {
                case Segment.DayOfWeek:
                    left = CronWordHelper.DayOfWeek(node.Left.Token.Value).AsInt();
                    right = CronWordHelper.DayOfWeek(node.Right.Token.Value).AsInt();
                    foreach (var dayOfWeek in ListExtension.Expand(left, right, 1))
                        Result[_lastSegment].Add(new NthDayOfMonthComputeList(Time, dayOfWeek.AsDayOfWeek()));
                    break;
                case Segment.Month:
                    left = CronWordHelper.Month(node.Left.Token.Value).AsInt();
                    right = CronWordHelper.Month(node.Right.Token.Value).AsInt();
                    skipNumericEvaluation = true;
                    goto default;
                default:
                    if (!skipNumericEvaluation)
                    {
                        left = node.Left.Evaluate(_lastSegment).First();
                        right = node.Right.Evaluate(_lastSegment).First();
                    }
                    var lastCount = Result[_lastSegment].Count;
                    var expandedSegmentValues = ListExtension.Expand(left, right, 1);
                    Result[_lastSegment].Add(
                        new PersistentList<int>(expandedSegmentValues));
                    Result[_lastSegment].SetRange(0, lastCount + expandedSegmentValues.Count);
                    break;
            }
        }

        public void Visit(WordNode node)
        {
            switch (_lastSegment)
            {
                case Segment.DayOfWeek:
                    Result[_lastSegment].Add(
                        new NthDayOfMonthComputeList(Time, CronWordHelper.DayOfWeek(node.Token.Value))
                    );
                    break;
                case Segment.Month:
                    Result[_lastSegment].Add(
                        new PersistentList<int>(new List<int> {CronWordHelper.Month(node.Token.Value).AsInt()})
                    );
                    break;
            }
        }

        public void Visit(NumberNode node)
        {
        }

        public void Visit(QuestionMarkNode node)
        {
            switch (_lastSegment)
            {
                case Segment.DayOfWeek:
                    var list = new EverydayOfWeekComputeList(Time);
                    Result[_lastSegment].Add(list);
                    Result[_lastSegment].SetRange(0, list.Count - 1);
                    break;
            }
        }

        public void Visit(IncrementByNode node)
        {
        }

        public void Visit(LNode node)
        {
            switch (_lastSegment)
            {
                case Segment.DayOfMonth:
                    var token = node.Token as LToken;
                    //Default value of the token is 1 but L in this segment can be placed as L without 1 (eg.1L). But compute list operates on values from 0 to n
                    //TO DO: is it correct?????? 
                    var number = int.Parse(node.Token.Value);
                    if (number == 1)
                        number = 0;
                    Result[_lastSegment].Add(new MonthBasedComputeList(Time, new[] {number}));
                    Result[_lastSegment].SetRange(0, Result[_lastSegment].Count);
                    break;
                case Segment.DayOfWeek:
                    Result[_lastSegment].Add(new LastDayOfWeekInMonthBasedOnCurrentMonthComputeList(Time,
                        new[] {int.Parse(node.Token.Value)}));
                    Result[_lastSegment].SetRange(0, Result[_lastSegment].Count);
                    break;
            }
        }

        public void Visit(WNode node)
        {
            switch (_lastSegment)
            {
                case Segment.DayOfMonth:
                    Result[_lastSegment].Add(new NearWeekdayComputeList(Time, int.Parse(node.Token.Value)));
                    break;
            }
        }

        public void Visit(HashNode node)
        {
            var dayOfWeek = CronWordHelper.DayOfWeek(node.Left.Token.Value);
            var nthOfMonth = int.Parse(node.Right.Token.Value);
            Result[_lastSegment].Add(new NthDayOfMonthLimitedByNumberOfWeekComputeList(Time, dayOfWeek, nthOfMonth));
            Result[_lastSegment].SetRange(0, Result[_lastSegment].Count);
        }

        public virtual void Visit(EndOfFileNode node)
        {
        }

        public void Visit(NumericPrecededLNode node)
        {
            var count = Result[_lastSegment].Count;
            switch (_lastSegment)
            {
                case Segment.DayOfMonth:
                    Result[_lastSegment].Add(new MonthBasedComputeList(Time, new[] {int.Parse(node.Token.Value)}));
                    Result[_lastSegment].SetRange(0, count);
                    break;
                case Segment.DayOfWeek:
                    Result[_lastSegment].Add(new LastDayOfWeekInMonthBasedOnCurrentMonthComputeList(Time,
                        new[] {int.Parse(node.Token.Value)}));
                    Result[_lastSegment].SetRange(0, count);
                    break;
            }
        }

        public void Visit(NumericPrecededWNode node)
        {
            switch (_lastSegment)
            {
                case Segment.DayOfMonth:
                    Result[_lastSegment].Add(new NearWeekdayComputeList(Time, int.Parse(node.Token.Value)));
                    break;
            }
        }

        public void Visit(LwNode node)
        {
            switch (_lastSegment)
            {
                case Segment.DayOfMonth:
                    Result[_lastSegment].Add(new LastWeekdayOfMonthComputeList(Time));
                    break;
            }
        }

        public void Visit(MissingNode node)
        {
        }
    }
}