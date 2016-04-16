using System;
using System.Collections.Generic;
using Cron.Parser.Nodes;
using Cron.Parser.Enums;
using Cron.Parser.Exceptions;
using Cron.Parser.Extensions;
using Cron.Parser.Visitors;
using Cron.Visitors.Exceptions;
using System.Linq;

namespace Cron.Visitors
{
    public class CronRulesNodeVisitor : INodeVisitor
    {
        private readonly List<Exception> errors;
        private readonly List<SyntaxError> syntaxErrors;
        private SyntaxNode parent;
        private Segment segment;
        private short segmentsCount;

        public CronRulesNodeVisitor()
        {
            errors = new List<Exception>();
            syntaxErrors = new List<SyntaxError>();
            segmentsCount = 0;
        }

        public virtual bool IsValid => errors.Count == 0 && syntaxErrors.Count == 0;

        public virtual IEnumerable<Exception> ValidationErrors => errors;

        public virtual IEnumerable<SyntaxError> SyntaxErrors => syntaxErrors;

        public virtual void Visit(SegmentNode node)
        {
            parent = node;
            segment = node.Segment;
            segmentsCount += 1;
        }

        public virtual void Visit(RangeNode node)
        {
            try
            {
                var hasUnsupportedLeftValue = false;
                var hasUnsupportedRightValue = false;
                var items = node.Desecendants;
                ReportIfNodesCountMismatched(items, node);
                switch (segment)
                {
                    case Segment.Seconds:
                        {
                            hasUnsupportedLeftValue |= ReportIfFieldValueOfUnsupportedType(items[0], TokenType.Integer);
                            hasUnsupportedRightValue |= ReportIfFieldValueOfUnsupportedType(items[1], TokenType.Integer);
                            if (!hasUnsupportedLeftValue && items[0].Token.TokenType != TokenType.Range && items[0].Token.TokenType != TokenType.Hash)
                            {
                                ReportIfSecondIsOutOfRange(items[0]);
                                ReportIfNumericRangesSwaped(items, node);
                            }
                            if (!hasUnsupportedRightValue) ReportIfSecondIsOutOfRange(items[1]);
                            break;
                        }
                    case Segment.Minutes:
                        {
                            hasUnsupportedLeftValue |= ReportIfFieldValueOfUnsupportedType(items[0], TokenType.Integer);
                            hasUnsupportedRightValue |= ReportIfFieldValueOfUnsupportedType(items[1], TokenType.Integer);
                            if (!hasUnsupportedLeftValue && items[0].Token.TokenType != TokenType.Range)
                            {
                                ReportIfMinuteIsOutOfRange(items[0]);
                                ReportIfNumericRangesSwaped(items, node);
                            }
                            if (!hasUnsupportedRightValue) ReportIfMinuteIsOutOfRange(items[1]);
                            break;
                        }
                    case Segment.Hours:
                        {
                            hasUnsupportedLeftValue |= ReportIfFieldValueOfUnsupportedType(items[0], TokenType.Integer);
                            hasUnsupportedRightValue |= ReportIfFieldValueOfUnsupportedType(items[1], TokenType.Integer);
                            if (!hasUnsupportedLeftValue && items[0].Token.TokenType != TokenType.Range)
                            {
                                ReportIfHourIsOutOfRange(items[0]);
                                ReportIfNumericRangesSwaped(items, node);
                            }
                            if (!hasUnsupportedRightValue) ReportIfHourIsOutOfRange(items[1]);
                            break;
                        }
                    case Segment.DayOfMonth:
                        {
                            hasUnsupportedLeftValue |= ReportIfFieldValueOfUnsupportedType(items[0], TokenType.Integer);
                            hasUnsupportedRightValue |= ReportIfFieldValueOfUnsupportedType(items[1], TokenType.Integer);
                            if (items[0].Token.TokenType != TokenType.Range)
                            {
                                ReportIfDayOfMonthIsOutOfRange(items[0]);
                                ReportIfNumericRangesSwaped(items, node);
                            }
                            if (!hasUnsupportedRightValue) ReportIfDayOfMonthIsOutOfRange(items[1]);
                            break;
                        }
                    case Segment.Year:
                        {
                            hasUnsupportedLeftValue |= ReportIfFieldValueOfUnsupportedType(items[0], TokenType.Integer);
                            hasUnsupportedRightValue |= ReportIfFieldValueOfUnsupportedType(items[1], TokenType.Integer);
                            if (items[0].Token.TokenType != TokenType.Range)
                            {
                                ReportIfYearIsOutOfRange(items[0]);
                                ReportIfNumericRangesSwaped(items, node);
                            }
                            if (!hasUnsupportedRightValue) ReportIfYearIsOutOfRange(items[1]);
                            break;
                        }
                    case Segment.DayOfWeek:
                        {
                            hasUnsupportedLeftValue |= ReportIfFieldValueOfUnsupportedType(items[0], TokenType.Integer, TokenType.Name);
                            hasUnsupportedRightValue |= ReportIfFieldValueOfUnsupportedType(items[1], TokenType.Integer, TokenType.Name);
                            if (items[0].Token.TokenType != TokenType.Range)
                            {
                                ReportIfDayOfWeekIsOutOfRange(items[0]);
                                ReportIfDayOfWeekRangesSwaped(items, node);
                            }
                            if (!hasUnsupportedRightValue) ReportIfDayOfWeekIsOutOfRange(items[1]);
                            break;
                        }
                    case Segment.Month:
                        {
                            hasUnsupportedLeftValue |= ReportIfFieldValueOfUnsupportedType(items[0], TokenType.Integer, TokenType.Name);
                            hasUnsupportedRightValue |= ReportIfFieldValueOfUnsupportedType(items[1], TokenType.Integer, TokenType.Name);
                            if (items[0].Token.TokenType != TokenType.Range)
                            {
                                ReportIfMonthIsOutOfRange(items[0]);
                                ReportIfMonthRangesSwaped(items, node);
                            }
                            if (!hasUnsupportedRightValue) ReportIfMonthIsOutOfRange(items[1]);
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
                errors.Add(new RangeNodeException(exc));
            }
        }

        private void ReportIfDayOfWeekRangesSwaped(SyntaxNode[] items, RangeNode node)
        {
            ReportIfRangesSwaped<int>(items, node, (a, b) => {
                var left = CronWordHelper.DayOfWeek(a);
                var right = CronWordHelper.DayOfWeek(b);
                if(left > right)
                {
                    return true;
                }
                return false;
            });
        }

        private void ReportIfMonthRangesSwaped(SyntaxNode[] items, RangeNode node)
        {
            ReportIfRangesSwaped<int>(items, node, (a, b) => {
                var left = CronWordHelper.Month(a);
                var right = CronWordHelper.Month(b);
                if(left > right)
                {
                    return true;
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
                syntaxErrors.Add(new SyntaxError(items.Select(f => f.FullSpan).ToArray(), segment, string.Format(Properties.Resources.RangeValueSwapped, items[0].Token.Value, items[1].Token.Value), SyntaxErrorKind.SwappedValue));
            }
        }

        public virtual void Visit(WordNode node)
        {
            try
            {
                switch (segment)
                {
                    case Segment.Seconds:
                    case Segment.Hours:
                    case Segment.Minutes:
                    case Segment.DayOfMonth:
                    case Segment.Year:
                        throw new UnexpectedWordNodeAtSegment(node.Token, segment);
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
                errors.Add(exc);
            }
        }

        public virtual void Visit(QuestionMarkNode node)
        {
            try
            {
                switch (segment)
                {
                    case Segment.DayOfMonth:
                    case Segment.DayOfWeek:
                        return;
                }
                throw new UnexpectedQuestionMarkAtSegment(node.Token, segment);
            }
            catch (BaseCronValidationException exc)
            {
                errors.Add(exc);
            }
        }

        public virtual void Visit(LNode node)
        {
            try
            {
                switch (segment)
                {
                    case Segment.DayOfMonth:
                        break;
                    case Segment.DayOfWeek:
                        break;
                    default:
                        throw new UnexpectedLNodeAtSegment(node.Token, segment);
                }
            }
            catch (BaseCronValidationException exc)
            {
                errors.Add(exc);
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
                        throw new UnexpectedPrecededLNodeAtSegmentException();
                }
            }
            catch (BaseCronValidationException exc)
            {
                errors.Add(exc);
            }
        }

        public virtual void Visit(NumericPrecededWNode node)
        {
            try
            {
                switch (segment)
                {
                    case Segment.DayOfMonth:
                        ReportIfWNodeAmongOtherValues(node);
                        ReportIfDayOfWeekIsOutOfRange(node);
                        return;
                }
                throw new UnexpectedPrecededWNodeAtSegmentException(node.Token, segment);
            }
            catch (BaseCronValidationException exc)
            {
                errors.Add(exc);
            }
        }

        public virtual void Visit(HashNode node)
        {
            try
            {
                switch (segment)
                {
                    case Segment.DayOfWeek:
                        ReportIfHashNodeOutOfRange(node);
                        break;
                    default:
                        throw new UnexpectedHashNodeAtSegment(node.Token, segment);
                }
            }
            catch (BaseCronValidationException exc)
            {
                errors.Add(exc);
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
                errors.Add(exc);
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
                errors.Add(exc);
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
                errors.Add(exc);
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
                errors.Add(exc);
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
                errors.Add(exc);
            }
        }

        public virtual void Visit(RootComponentNode node)
        { }

        public virtual void Visit(StarNode node)
        { }

        public virtual void Visit(CommaNode node)
        { }

        public void Visit(MissingNode node)
        { }

        private bool ReportIfFieldValueOfUnsupportedType(SyntaxNode node, params TokenType[] supportedTypes)
        {
            if(!supportedTypes.Contains(node.Token.TokenType))
            {
                syntaxErrors.Add(new SyntaxError(node.FullSpan, segment,
                    string.Format(Properties.Resources.UnsupportedFieldValue, node.ToString(), node.Token.TokenType, string.Join(",", supportedTypes)), SyntaxErrorKind.UnsupportedValue));
                return true;
            }
            return false;
        }

        private void ReportIfDayOfMonthIsOutOfRange(SyntaxNode node)
        {
            if (node.Token.TokenType == TokenType.Integer)
            {
                ReportIfOutOfRange(1, 32, node, "dayInMonth");
                return;
            }
            syntaxErrors.Add(new SyntaxError(node.FullSpan, segment,
                string.Format(Properties.Resources.UnsupportedValue, node.Token.Value), SyntaxErrorKind.UnsupportedValue));
        }

        private void ReportIfDayOfWeekIsOutOfRange(SyntaxNode node)
        {
            if (!CronWordHelper.ContainsDayOfWeek(node.Token.Value))
            {
                syntaxErrors.Add(new SyntaxError(node.FullSpan, segment,
                    string.Format(Properties.Resources.UnsupportedValue, node.Token.Value), SyntaxErrorKind.UnsupportedValue));
            }
        }

        private void ReportIfHashNodeOutOfRange(HashNode node)
        {
            if (!CronWordHelper.ContainsDayOfWeek(node.Left.Token.Value))
            {
                syntaxErrors.Add(new SyntaxError(node.Left.FullSpan, segment,
                    string.Format(Properties.Resources.UnsupportedValue, node.Token.Value), SyntaxErrorKind.UnsupportedValue));
            }
            var weekOfMonth = int.Parse(node.Right.Token.Value);
            if (weekOfMonth < 0 || weekOfMonth > 4)
            {
                syntaxErrors.Add(new SyntaxError(node.Right.FullSpan, segment,
                    string.Format(Properties.Resources.UnsupportedValue, node.Token.Value), SyntaxErrorKind.UnsupportedValue));
            }
        }

        private void ReportIfHourIsOutOfRange(SyntaxNode node)
        {
            if (node.Token.TokenType == TokenType.Integer)
            {
                ReportIfOutOfRange(0, 23, node, "hour");
                return;
            }
            syntaxErrors.Add(new SyntaxError(node.FullSpan, segment, 
                string.Format(Properties.Resources.UnsupportedValue, node.Token.Value), SyntaxErrorKind.UnsupportedValue));
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
            syntaxErrors.Add(new SyntaxError(node.FullSpan, segment,
                string.Format(Properties.Resources.UnsupportedValue, node.Token.Value), SyntaxErrorKind.UnsupportedValue));
        }

        private void ReportIfMonthIsOutOfRange(SyntaxNode node)
        {
            if (!CronWordHelper.ContainsMonth(node.Token.Value))
            {
                syntaxErrors.Add(new SyntaxError(node.FullSpan, segment, Properties.Resources.MonthIsOutOfRange, SyntaxErrorKind.ValueOutOfRange));
            }
        }

        private void ReportIfNodesCountMismatched(SyntaxNode[] items, SyntaxNode parent)
        {
            if (items.Count() != 2)
            {
                syntaxErrors.Add(new SyntaxError(items.Select(f => f.FullSpan).ToArray(), segment,
                    string.Format(Properties.Resources.NodeCountMismatched, 2, items.Count()), SyntaxErrorKind.CountMismatched));
            }
        }

        private void ReportIfOutOfRange(int minValue, SyntaxNode node, string argName)
        {
            var value = int.Parse(node.Token.Value);
            if (value < minValue)
            {
                syntaxErrors.Add(new SyntaxError(node.FullSpan, segment, 
                    string.Format(Properties.Resources.OutOfRangeMin, value, segment, minValue), SyntaxErrorKind.ValueOutOfRange));
            }
        }

        private void ReportIfOutOfRange(int minValue, int maxValue, SyntaxNode node, string argName)
        {
            var value = int.Parse(node.Token.Value);
            if (value < minValue || value > maxValue)
            {
                syntaxErrors.Add(new SyntaxError(node.FullSpan, segment, 
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
            syntaxErrors.Add(new SyntaxError(node.FullSpan, segment,
                string.Format(Properties.Resources.UnsupportedValue, node.Token.Value), SyntaxErrorKind.UnsupportedValue));
        }

        private void ReportIfYearIsOutOfRange(SyntaxNode node)
        {
            if (node.Token.TokenType == TokenType.Integer)
            {
                ReportIfOutOfRange(1970, 3000, node, "year");
                return;
            }
            syntaxErrors.Add(new SyntaxError(node.FullSpan, segment,
                string.Format(Properties.Resources.UnsupportedValue, node.Token.Value), SyntaxErrorKind.UnsupportedValue));
        }

        private void ReportIfLWNodeAmongOtherValues(SyntaxNode node)
        {
            ReportIfMoreThanOneDescendants(node);
        }

        private void ReportIfMoreThanOneDescendants(SyntaxNode node)
        {
            if (parent.Desecendants.Count() != 1)
            {
                syntaxErrors.Add(new SyntaxError(parent.Desecendants.Select(f => f.FullSpan).ToArray(), segment,
                    string.Format(Properties.Resources.NodeCountMismatched, 1, parent.Desecendants.Count()), SyntaxErrorKind.CountMismatched));
            }
        }

        private void ReportIfWNodeAmongOtherValues(SyntaxNode node)
        {
            ReportIfMoreThanOneDescendants(node);
        }
    }
}
