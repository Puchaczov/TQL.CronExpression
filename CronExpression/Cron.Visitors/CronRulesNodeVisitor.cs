using System;
using System.Collections.Generic;
using Cron.Parser.Nodes;
using Cron.Parser.Enums;
using Cron.Parser.Exceptions;
using Cron.Parser.Extensions;
using Cron.Parser.Visitors;
using Cron.Visitors.Exceptions;
using System.Linq;
using Cron.Parser.Helpers;

namespace Cron.Visitors
{
    public class CronRulesNodeVisitor : INodeVisitor
    {
        private readonly List<Exception> criticalErrors;
        private readonly List<Error> errors;
        private SyntaxNode parent;
        private Segment segment;
        private SegmentNode currentSegment;
        private short segmentsCount;

        public CronRulesNodeVisitor()
        {
            criticalErrors = new List<Exception>();
            errors = new List<Error>();
            segmentsCount = 0;
        }

        public virtual bool IsValid => criticalErrors.Count == 0 && errors.Count == 0;

        public virtual IEnumerable<Exception> ValidationErrors => criticalErrors;

        public virtual IEnumerable<Error> SyntaxErrors => errors;

        public object TreeHelper { get; private set; }

        public virtual void Visit(SegmentNode node)
        {
            parent = node;
            currentSegment = node;
            segment = node.Segment;
            segmentsCount += 1;
        }

        public virtual void Visit(RangeNode node)
        {
            try
            {
                var items = node.Desecendants;
                ReportIfNodesCountMismatched(items, node);
                switch (segment)
                {
                    case Segment.Seconds:
                        {
                            CheckRangeNode(node, ReportIfNumericRangesSwaped, ReportIfSecondIsOutOfRange, TokenType.Integer);
                            break;
                        }
                    case Segment.Minutes:
                        {
                            CheckRangeNode(node, ReportIfNumericRangesSwaped, ReportIfMinuteIsOutOfRange, TokenType.Integer);
                            break;
                        }
                    case Segment.Hours:
                        {
                            CheckRangeNode(node, ReportIfNumericRangesSwaped, ReportIfHourIsOutOfRange, TokenType.Integer);
                            break;
                        }
                    case Segment.DayOfMonth:
                        {
                            CheckRangeNode(node, ReportIfNumericRangesSwaped, ReportIfDayOfMonthIsOutOfRange, TokenType.Integer);
                            break;
                        }
                    case Segment.Year:
                        {
                            CheckRangeNode(node, ReportIfNumericRangesSwaped, ReportIfYearIsOutOfRange, TokenType.Integer);
                            break;
                        }
                    case Segment.DayOfWeek:
                        {
                            CheckRangeNode(node, ReportIfDayOfWeekRangesSwaped, ReportIfDayOfWeekIsOutOfRange, TokenType.Integer, TokenType.Name);
                            break;
                        }
                    case Segment.Month:
                        {
                            CheckRangeNode(node, ReportIfMonthRangesSwaped, ReportIfMonthIsOutOfRange, TokenType.Integer, TokenType.Name);
                            break;
                        }
                    default:
                        {
                            throw new UnexpectedSegmentException(segment);
                        }
                }
            }
            catch (BaseCronValidationException exc)
            {
                criticalErrors.Add(new RangeNodeException(exc));
            }
        }

        private void CheckRangeNode(RangeNode node, Action<SyntaxNode[], RangeNode> action, Action<SyntaxNode> actionCheckOutOfRange, params TokenType[] types)
        {
            var hasUnsupportedLeftValue = false;
            var hasUnsupportedRightValue = false;
            var items = node.Desecendants;
            if (items[0].Token.TokenType == TokenType.Missing)
            {
                items[0].Accept(this);
                hasUnsupportedLeftValue = true;
            }
            else
            {
                hasUnsupportedLeftValue |= ReportIfFieldValueOfUnsupportedType(items[0], types);
            }
            if (items[1].Token.TokenType == TokenType.Missing)
            {
                items[1].Accept(this);
                hasUnsupportedRightValue = true;
            }
            else
            {
                hasUnsupportedRightValue |= ReportIfFieldValueOfUnsupportedType(items[1], types);
            }
            if (!hasUnsupportedLeftValue)
            {
                actionCheckOutOfRange?.Invoke(items[0]);
            }
            if (!hasUnsupportedRightValue)
            {
                actionCheckOutOfRange?.Invoke(items[1]);
            }
            if (!hasUnsupportedLeftValue && !hasUnsupportedRightValue)
            {
                action?.Invoke(items, node);
            }
        }

        private void ReportIfDayOfWeekRangesSwaped(SyntaxNode[] items, RangeNode node)
        {
            ReportIfRangesSwaped<int>(items, node, (a, b) => {
                if(CronWordHelper.ContainsDayOfWeek(a) && CronWordHelper.ContainsDayOfWeek(b))
                {
                    var left = CronWordHelper.DayOfWeek(a);
                    var right = CronWordHelper.DayOfWeek(b);
                    if (left > right)
                    {
                        return true;
                    }
                }
                return false;
            });
        }

        private void ReportIfMonthRangesSwaped(SyntaxNode[] items, RangeNode node)
        {
            ReportIfRangesSwaped<int>(items, node, (a, b) => {
                if(CronWordHelper.ContainsMonth(a) && CronWordHelper.ContainsMonth(b))
                {
                    var left = CronWordHelper.Month(a);
                    var right = CronWordHelper.Month(b);
                    if (left > right)
                    {
                        return true;
                    }
                }
                return false;
            });
        }

        private void ReportIfNumericRangesSwaped(SyntaxNode[] items, RangeNode node)
        {
            ReportIfRangesSwaped<int>(items, node, (a, b) => {
                var left = int.Parse(a);
                var right = int.Parse(b);
                if(left > right)
                {
                    return true;
                }
                return false;
            });
        }

        private void ReportIfRangesSwaped<T>(SyntaxNode[] items, RangeNode node, Func<string, string, bool> compareAction)
        {
            if(items[0].Token.TokenType != TokenType.Integer && items[0].Token.TokenType != TokenType.Name)
            {
                return;
            }
            if(compareAction?.Invoke(items[0].Token.Value, items[1].Token.Value) ?? default(bool))
            {
                errors.Add(new SyntaxError(items.Select(f => f.FullSpan).ToArray(), segment, string.Format(Properties.Resources.RangeValueSwapped, items[0].Token.Value, items[1].Token.Value), SyntaxErrorKind.SwappedValue));
            }
        }

        public virtual void Visit(WordNode node)
        {
            try
            {
                switch (segment)
                {
                    case Segment.Seconds:
                        errors.Add(new SyntaxError(node.FullSpan, segment,
                            string.Format(Properties.Resources.UnsupportedFieldValue, node.Token.Value, node.Token.TokenType,
                            string.Join(",", TokenType.Integer, TokenType.Star, TokenType.Comma, TokenType.Range)), SyntaxErrorKind.UnsupportedValue));
                        break;
                    case Segment.Hours:
                        errors.Add(new SyntaxError(node.FullSpan, segment,
                            string.Format(Properties.Resources.UnsupportedFieldValue, node.Token.Value, node.Token.TokenType,
                            string.Join(",", TokenType.Integer, TokenType.Star, TokenType.Comma, TokenType.Range)), SyntaxErrorKind.UnsupportedValue));
                        break;
                    case Segment.Minutes:
                        errors.Add(new SyntaxError(node.FullSpan, segment,
                            string.Format(Properties.Resources.UnsupportedFieldValue, node.Token.Value, node.Token.TokenType,
                            string.Join(",", TokenType.Integer, TokenType.Star, TokenType.Comma, TokenType.Range)), SyntaxErrorKind.UnsupportedValue));
                        break;
                    case Segment.DayOfMonth:
                        errors.Add(new SyntaxError(node.FullSpan, segment,
                            string.Format(Properties.Resources.UnsupportedFieldValue, node.Token.Value, node.Token.TokenType,
                            string.Join(",", TokenType.Integer, TokenType.Star, TokenType.Comma, TokenType.Range, TokenType.QuestionMark, TokenType.L, TokenType.W)), SyntaxErrorKind.UnsupportedValue));
                        break;
                    case Segment.Year:
                        errors.Add(new SyntaxError(node.FullSpan, segment,
                            string.Format(Properties.Resources.UnsupportedFieldValue, node.Token.Value, node.Token.TokenType,
                            string.Join(",", TokenType.Integer, TokenType.Star, TokenType.Comma, TokenType.Range)), SyntaxErrorKind.UnsupportedValue));
                        break;
                }
                switch (segment)
                {
                    case Segment.Month:
                        ReportIfMonthIsOutOfRange(node);
                        break;
                    case Segment.DayOfWeek:
                        ReportIfDayOfWeekIsOutOfRange(node);
                        break;
                }
            }
            catch (BaseCronValidationException exc)
            {
                criticalErrors.Add(exc);
            }
        }

        public virtual void Visit(QuestionMarkNode node)
        {
            try
            {
                switch (segment)
                {
                    case Segment.Seconds:
                    case Segment.Minutes:
                    case Segment.Hours:
                    case Segment.Month:
                    case Segment.Year:
                        errors.Add(new SyntaxError(node.FullSpan, segment,
                            string.Format(Properties.Resources.UnsupportedFieldValue, node.Token.Value, node.Token.TokenType,
                            string.Join(",", TokenType.Integer, TokenType.Star, TokenType.Comma, TokenType.Range)), SyntaxErrorKind.UnsupportedValue));
                        break;
                    case Segment.DayOfMonth:
                    case Segment.DayOfWeek:
                        var siblings = TreeHelpers.Siblings(currentSegment, node);
                        if(siblings.Count() > 0)
                        {
                            errors.Add(new SyntaxError(node.FullSpan, segment, string.Format(Properties.Resources.CannotBeUsedInThisContext, node.Token.Value, Properties.Resources.DoNotMixValues), SyntaxErrorKind.UnsupportedValue));
                        }
                        return;
                }
            }
            catch (BaseCronValidationException exc)
            {
                criticalErrors.Add(exc);
            }
        }

        public virtual void Visit(LNode node)
        {
            try
            {
                switch (segment)
                {
                    case Segment.DayOfMonth:
                    case Segment.DayOfWeek:
                        break;
                    default:
                        errors.Add(new SyntaxError(node.FullSpan, segment,
                            string.Format(Properties.Resources.UnsupportedFieldValue, node.Token.Value, node.Token.TokenType,
                            string.Join(",", TokenType.Integer, TokenType.Star, TokenType.Comma, TokenType.Range)), SyntaxErrorKind.UnsupportedValue));
                        break;
                }
            }
            catch (BaseCronValidationException exc)
            {
                criticalErrors.Add(exc);
            }
        }

        public virtual void Visit(NumericPrecededLNode node)
        {
            try
            {
                switch (segment)
                {
                    case Segment.DayOfMonth:
                        ReportIfDayOfMonthIsOutOfRange(node);
                        break;
                    case Segment.DayOfWeek:
                        ReportIfDayOfWeekIsOutOfRange(node);
                        break;
                    default:
                        errors.Add(new SyntaxError(node.FullSpan, segment,
                            string.Format(Properties.Resources.UnsupportedFieldValue, node.Token.Value, node.Token.TokenType,
                            string.Join(",", TokenType.Integer, TokenType.Star, TokenType.Comma, TokenType.Range)), SyntaxErrorKind.UnsupportedValue));
                        break;
                }
            }
            catch (BaseCronValidationException exc)
            {
                criticalErrors.Add(exc);
            }
        }

        public virtual void Visit(NumericPrecededWNode node)
        {
            try
            {
                var supportedTypes = string.Empty;
                switch (segment)
                {
                    case Segment.DayOfMonth:
                        ReportIfWNodeAmongOtherValues(node);
                        ReportIfDayOfWeekIsOutOfRange(node);
                        return;
                    case Segment.DayOfWeek:
                        supportedTypes = string.Join(",", TokenType.Integer, TokenType.Star, TokenType.Comma, TokenType.Range, TokenType.L, TokenType.Hash, TokenType.QuestionMark);
                        break;
                    default:
                        supportedTypes = string.Join(",", TokenType.Integer, TokenType.Star, TokenType.Comma, TokenType.Range);
                        break;
                }
                errors.Add(new SyntaxError(node.FullSpan, segment,
                    string.Format(Properties.Resources.UnsupportedFieldValue, node.Token.Value, node.Token.TokenType,
                    supportedTypes), SyntaxErrorKind.UnsupportedValue));
            }
            catch (BaseCronValidationException exc)
            {
                criticalErrors.Add(exc);
            }
        }

        public virtual void Visit(HashNode node)
        {
            try
            {
                var supportedTypes = string.Empty;
                switch (segment)
                {
                    case Segment.DayOfWeek:
                        ReportIfHashNodeOutOfRange(node);
                        return;
                    case Segment.DayOfMonth:
                        supportedTypes = string.Join(",", TokenType.Integer, TokenType.Star, TokenType.Comma, TokenType.Range, TokenType.L, TokenType.W, TokenType.QuestionMark);
                        break;
                    default:
                        supportedTypes = string.Join(",", TokenType.Integer, TokenType.Star, TokenType.Comma, TokenType.Range);
                        break;
                }
                errors.Add(new SyntaxError(node.FullSpan, segment,
                    string.Format(Properties.Resources.UnsupportedFieldValue, node.Token.Value, node.Token.TokenType,
                    supportedTypes), SyntaxErrorKind.UnsupportedValue));
            }
            catch (BaseCronValidationException exc)
            {
                criticalErrors.Add(exc);
            }
        }

        public virtual void Visit(EndOfFileNode node)
        {
            segmentsCount += 1;
            try
            {
                if (segmentsCount != 8)
                    throw new ExpressionTooShortException(node.Token, segment);
            }
            catch (BaseCronValidationException exc)
            {
                criticalErrors.Add(exc);
            }
        }

        public virtual void Visit(WNode node)
        {
            try
            {
                switch (segment)
                {
                    case Segment.DayOfMonth:
                        ReportIfWNodeAmongOtherValues(node);
                        break;
                    default:
                        throw new UnexpectedWNodeAtSegment(node.Token, segment);
                }
            }
            catch (BaseCronValidationException exc)
            {
                criticalErrors.Add(exc);
            }
        }

        public virtual void Visit(LWNode node)
        {
            try
            {
                switch (segment)
                {
                    case Segment.DayOfMonth:
                        ReportIfLWNodeAmongOtherValues(node);
                        break;
                    default:
                        throw new UnexpectedLWNodeAtSegment(node.Token, segment);
                }
            }
            catch (BaseCronValidationException exc)
            {
                criticalErrors.Add(exc);
            }
        }

        public virtual void Visit(NumberNode node)
        {
            try
            {
                switch (segment)
                {
                    case Segment.Seconds:
                        ReportIfSecondIsOutOfRange(node);
                        break;
                    case Segment.Minutes:
                        ReportIfMinuteIsOutOfRange(node);
                        break;
                    case Segment.Hours:
                        ReportIfHourIsOutOfRange(node);
                        break;
                    case Segment.DayOfMonth:
                        ReportIfDayOfMonthIsOutOfRange(node);
                        break;
                    case Segment.Month:
                        ReportIfMonthIsOutOfRange(node);
                        break;
                    case Segment.DayOfWeek:
                        ReportIfDayOfWeekIsOutOfRange(node);
                        break;
                    case Segment.Year:
                        ReportIfYearIsOutOfRange(node);
                        break;
                    default:
                        throw new UnexpectedSegmentException(segment);
                }
            }
            catch (BaseCronValidationException exc)
            {
                criticalErrors.Add(exc);
            }
        }

        public virtual void Visit(IncrementByNode node)
        {
            try
            {
                switch (segment)
                {
                    case Segment.Seconds:
                        ReportIfLessThanZero(node.Right, nameof(node.Right));
                        if (node.Left.Token.TokenType != TokenType.Range)
                        {
                            ReportIfSecondIsOutOfRange(node.Left);
                        }
                        break;
                    case Segment.Minutes:
                        ReportIfLessThanZero(node.Right, nameof(node.Right));
                        if (node.Left.Token.TokenType != TokenType.Range)
                        {
                            ReportIfMinuteIsOutOfRange(node.Left);
                        }
                        break;
                    case Segment.Hours:
                        ReportIfLessThanZero(node.Right, nameof(node.Right));
                        if (node.Left.Token.TokenType != TokenType.Range)
                        {
                            ReportIfHourIsOutOfRange(node.Left);
                        }
                        break;
                    case Segment.DayOfMonth:
                        ReportIfLessThanZero(node.Right, nameof(node.Right));
                        if (node.Left.Token.TokenType != TokenType.Range)
                        {
                            ReportIfDayOfMonthIsOutOfRange(node.Left);
                        }
                        break;
                    case Segment.Month:
                        ReportIfLessThanZero(node.Right, nameof(node.Right));
                        if (node.Left.Token.TokenType != TokenType.Range)
                        {
                            ReportIfMonthIsOutOfRange(node.Left);
                        }
                        break;
                    case Segment.DayOfWeek:
                        ReportIfLessThanZero(node.Right, nameof(node.Right));
                        if (node.Left.Token.TokenType != TokenType.Range)
                        {
                            ReportIfDayOfWeekIsOutOfRange(node.Left);
                        }
                        break;
                    case Segment.Year:
                        ReportIfLessThanZero(node.Right, nameof(node.Right));
                        if (node.Left.Token.TokenType != TokenType.Range)
                        {
                            ReportIfYearIsOutOfRange(node.Left);
                        }
                        break;
                }
            }
            catch (BaseCronValidationException exc)
            {
                criticalErrors.Add(exc);
            }
        }

        public virtual void Visit(RootComponentNode node)
        { }

        public virtual void Visit(StarNode node)
        { }

        public virtual void Visit(CommaNode node)
        { }

        public void Visit(MissingNode node)
        {
            errors.Add(new SyntaxError(node.FullSpan, segment, 
                string.Format(Properties.Resources.MissingValue, segment), SyntaxErrorKind.MissingValue));
        }

        private bool ReportIfFieldValueOfUnsupportedType(SyntaxNode node, params TokenType[] supportedTypes)
        {
            if(!supportedTypes.Contains(node.Token.TokenType))
            {
                errors.Add(new SyntaxError(node.FullSpan, segment,
                    string.Format(Properties.Resources.UnsupportedFieldValue, node.ToString(), node.Token.TokenType, string.Join(",", supportedTypes)), SyntaxErrorKind.UnsupportedValue));
                return true;
            }
            return false;
        }

        private void ReportIfDayOfMonthIsOutOfRange(SyntaxNode node)
        {
            if (node.Token.TokenType == TokenType.Integer)
            {
                ReportIfOutOfRange(1, 31, node, "dayInMonth");
                return;
            }
            errors.Add(new SyntaxError(node.FullSpan, segment,
                string.Format(Properties.Resources.OutOfRange, node.Token.Value, segment, "0", "31"), SyntaxErrorKind.ValueOutOfRange));
        }

        private void ReportIfDayOfWeekIsOutOfRange(SyntaxNode node)
        {
            if (!CronWordHelper.ContainsDayOfWeek(node.Token.Value))
            {
                errors.Add(new SyntaxError(node.FullSpan, segment,
                    string.Format(Properties.Resources.OutOfRange, node.Token.Value, node.Token.TokenType, "1/MON", "7/SUN"), SyntaxErrorKind.ValueOutOfRange));
            }
        }

        private void ReportIfHashNodeOutOfRange(HashNode node)
        {
            if (!CronWordHelper.ContainsDayOfWeek(node.Left.Token.Value))
            {
                errors.Add(new SyntaxError(node.Left.FullSpan, segment,
                    string.Format(Properties.Resources.OutOfRange, node.Token.Value, segment, "0", "31"), SyntaxErrorKind.ValueOutOfRange));
            }
            var weekOfMonth = int.Parse(node.Right.Token.Value);
            if (weekOfMonth < 1 || weekOfMonth > 4)
            {
                errors.Add(new SyntaxError(node.Right.FullSpan, segment,
                    string.Format(Properties.Resources.OutOfRange, node.Token.Value, segment, "1", "4"), SyntaxErrorKind.ValueOutOfRange));
            }
        }

        private void ReportIfHourIsOutOfRange(SyntaxNode node)
        {
            if (node.Token.TokenType == TokenType.Integer)
            {
                ReportIfOutOfRange(0, 23, node, "hour");
                return;
            }
            errors.Add(new SyntaxError(node.FullSpan, segment,
                string.Format(Properties.Resources.OutOfRange, node.Token.Value, segment, "0", "23"), SyntaxErrorKind.ValueOutOfRange));
        }

        private void ReportIfLessThanZero(SyntaxNode node, string argName)
        {
            ReportIfOutOfRange(0, node, argName);
        }

        private void ReportIfMinuteIsOutOfRange(SyntaxNode node)
        {
            if (node.Token.TokenType == TokenType.Integer)
            {
                ReportIfOutOfRange(0, 59, node, "minute");
                return;
            }
            errors.Add(new SyntaxError(node.FullSpan, segment,
                string.Format(Properties.Resources.OutOfRange, node.Token.Value, segment, "0", "59"), SyntaxErrorKind.ValueOutOfRange));
        }

        private void ReportIfMonthIsOutOfRange(SyntaxNode node)
        {
            if (!CronWordHelper.ContainsMonth(node.Token.Value))
            {
                errors.Add(new SyntaxError(node.FullSpan, segment, string.Format(Properties.Resources.OutOfRange, node.Token.Value, segment, "1/JAN", "12/DEC"), SyntaxErrorKind.ValueOutOfRange));
            }
        }

        private void ReportIfNodesCountMismatched(SyntaxNode[] items, SyntaxNode parent)
        {
            if (items.Count() != 2)
            {
                errors.Add(new SyntaxError(items.Select(f => f.FullSpan).ToArray(), segment,
                    string.Format(Properties.Resources.NodeCountMismatched, 2, items.Count()), SyntaxErrorKind.CountMismatched));
            }
        }

        private void ReportIfOutOfRange(int minValue, SyntaxNode node, string argName)
        {
            var value = int.Parse(node.Token.Value);
            if (value < minValue)
            {
                errors.Add(new SyntaxError(node.FullSpan, segment,
                    string.Format(Properties.Resources.OutOfRangeMin, value, segment, minValue), SyntaxErrorKind.ValueOutOfRange));
            }
        }

        private void ReportIfOutOfRange(int minValue, int maxValue, SyntaxNode node, string argName)
        {
            var value = int.Parse(node.Token.Value);
            if (value < minValue || value > maxValue)
            {
                errors.Add(new SyntaxError(node.FullSpan, segment,
                    string.Format(Properties.Resources.OutOfRange, value, segment, minValue, maxValue), SyntaxErrorKind.ValueOutOfRange));
            }
        }

        private void ReportIfSecondIsOutOfRange(SyntaxNode node)
        {
            if (node.Token.TokenType == TokenType.Integer)
            {
                ReportIfOutOfRange(0, 59, node, "second");
                return;
            }
            errors.Add(new SyntaxError(node.FullSpan, segment,
                string.Format(Properties.Resources.OutOfRange, node.Token.Value, segment, "0", "59"), SyntaxErrorKind.ValueOutOfRange));
        }

        private void ReportIfYearIsOutOfRange(SyntaxNode node)
        {
            if (node.Token.TokenType == TokenType.Integer)
            {
                ReportIfOutOfRange(1970, 3000, node, "year");
                return;
            }
            errors.Add(new SyntaxError(node.FullSpan, segment,
                string.Format(Properties.Resources.OutOfRange, node.Token.Value, segment, 1970, 3000), SyntaxErrorKind.ValueOutOfRange));
        }

        private void ReportIfLWNodeAmongOtherValues(SyntaxNode node)
        {
            ReportIfMoreThanOneDescendants(node);
        }

        private void ReportIfMoreThanOneDescendants(SyntaxNode node)
        {
            if (parent.Desecendants.Count() != 1)
            {
                errors.Add(new SyntaxError(parent.Desecendants.Select(f => f.FullSpan).ToArray(), segment,
                    string.Format(Properties.Resources.NodeCountMismatched, 1, parent.Desecendants.Count()), SyntaxErrorKind.CountMismatched));
            }
        }

        private void ReportIfWNodeAmongOtherValues(SyntaxNode node)
        {
            ReportIfMoreThanOneDescendants(node);
        }
    }
}
