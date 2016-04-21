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
using Cron.Parser.Tokens;

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
        private bool reportWhenExpressionTooShort;

        public CronRulesNodeVisitor(bool reportWhenExpressionTooShort = true)
        {
            criticalErrors = new List<Exception>();
            errors = new List<Error>();
            segmentsCount = 0;
            this.reportWhenExpressionTooShort = reportWhenExpressionTooShort;
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
                AddSemanticError(
                    items.Select(f => f.FullSpan).ToArray(),
                    string.Format(Properties.Resources.RangeValueSwapped, items[0].Token.Value, items[1].Token.Value), 
                    SemanticErrorKind.SwappedValue);
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
                        AddSemanticError(
                            node.FullSpan,
                            string.Format(Properties.Resources.UnsupportedFieldValue, node.Token.Value, node.Token.TokenType, string.Join(",", GetSupportedTypes(segment))),
                            SemanticErrorKind.UnsupportedValue);
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
                        AddSemanticError(
                            node.FullSpan,
                            string.Format(Properties.Resources.UnsupportedFieldValue, node.Token.Value, node.Token.TokenType, string.Join(",", GetSupportedTypes(segment))),
                            SemanticErrorKind.UnsupportedValue);
                        break;
                    case Segment.DayOfMonth:
                    case Segment.DayOfWeek:
                        var siblings = TreeHelpers.Siblings(currentSegment, node);
                        if(siblings.Count() > 0)
                        {
                            AddSemanticError(
                                node.FullSpan,
                                string.Format(Properties.Resources.CannotBeUsedInThisContext, node.Token.Value, Properties.Resources.DoNotMixValues),
                                SemanticErrorKind.UnsupportedValue);
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
                        AddSemanticError(
                            node.FullSpan,
                            string.Format(Properties.Resources.UnsupportedFieldValue, node.Token.Value, node.Token.TokenType, string.Join(",", GetSupportedTypes(segment))),
                            SemanticErrorKind.UnsupportedValue);
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
                        AddSemanticError(
                            node.FullSpan,
                            string.Format(Properties.Resources.UnsupportedFieldValue, node.Token.Value, node.Token.TokenType, string.Join(",", GetSupportedTypes(segment))),
                            SemanticErrorKind.UnsupportedValue);
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
                switch (segment)
                {
                    case Segment.DayOfMonth:
                        ReportIfWNodeAmongOtherValues(node);
                        ReportIfDayOfWeekIsOutOfRange(node);
                        return;
                }
                AddSemanticError(
                    node.FullSpan,
                    string.Format(Properties.Resources.UnsupportedFieldValue, node.Token.Value, node.Token.TokenType, string.Join(",", GetSupportedTypes(segment))),
                    SemanticErrorKind.UnsupportedValue);
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
                switch (segment)
                {
                    case Segment.DayOfWeek:
                        ReportIfHashNodeOutOfRange(node);
                        return;
                }
                AddSemanticError(
                    node.FullSpan,
                    string.Format(Properties.Resources.UnsupportedFieldValue, node.Token.Value, node.Token.TokenType, string.Join(",", GetSupportedTypes(segment))),
                    SemanticErrorKind.UnsupportedValue);
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
                if(reportWhenExpressionTooShort && segmentsCount != 8)
                {
                    errors.Add(new SyntaxError(node.FullSpan, segment, Properties.Resources.UnexpectedEndOfFile, SyntaxErrorKind.MissingValue));
                }
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
                        var siblings = TreeHelpers.Siblings(currentSegment, node);
                        if(siblings.Count() != 0)
                        {
                            AddSemanticError(node.FullSpan,
                                string.Format(Properties.Resources.CannotBeUsedInThisContext, node.Token.Value, Properties.Resources.DoNotMixValues),
                                SemanticErrorKind.UnsupportedValue);
                        }
                        break;
                    default:
                        AddSemanticError(node.FullSpan,
                            string.Format(Properties.Resources.UnsupportedFieldValue, node.Token.Value, node.Token.TokenType, string.Join(",", GetSupportedTypes(segment))),
                            SemanticErrorKind.UnsupportedValue);
                        break;
                }
            }
            catch (BaseCronValidationException exc)
            {
                criticalErrors.Add(exc);
            }
        }

        private void AddSemanticError(TextSpan span, string message, SemanticErrorKind kind)
        {
            AddSemanticError(new TextSpan[] { span }, message, kind);
        }

        private void AddSemanticError(TextSpan[] spans, string message, SemanticErrorKind kind)
        {
            errors.Add(new SemanticError(spans, segment, message, kind));
        }

        private static TokenType[] GetSupportedTypes(Segment segment)
        {
            switch (segment)
            {
                case Segment.DayOfMonth:
                    return new TokenType[] { TokenType.Integer, TokenType.Star, TokenType.Comma, TokenType.Range, TokenType.L, TokenType.W, TokenType.QuestionMark };
                case Segment.DayOfWeek:
                    return new TokenType[] { TokenType.Integer, TokenType.Star, TokenType.Comma, TokenType.Range, TokenType.L, TokenType.Hash, TokenType.QuestionMark };
                case Segment.Unknown:
                    return new TokenType[] { };
                default:
                    return new TokenType[] { TokenType.Integer, TokenType.Star, TokenType.Comma, TokenType.Range };
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
                        AddSemanticError(
                            node.FullSpan,
                            string.Format(Properties.Resources.UnsupportedFieldValue, node.Token.Value, node.Token.TokenType, string.Join(",", GetSupportedTypes(segment))),
                            SemanticErrorKind.UnsupportedValue);
                        break;
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
                        AddSemanticError(
                            node.FullSpan,
                            string.Format(Properties.Resources.UnsupportedFieldValue, node.Token.Value, node.Token.TokenType, string.Join(",", GetSupportedTypes(segment))),
                            SemanticErrorKind.UnsupportedValue);
                        break;
                }
            }
            catch (BaseCronValidationException exc)
            {
                criticalErrors.Add(exc);
            }
        }

        public virtual void Visit(IncrementByNode node)
        {
            Visit(node, null, 1);
        }

        public virtual void Visit(IncrementByNode node, RangeNode range, int recurenceLevel)
        {
            try
            {
                switch (segment)
                {
                    case Segment.Seconds:
                        if(recurenceLevel == 1)
                        {
                            ReportIfLessThanZero(node.Right, nameof(node.Right));
                            ReportIfSecondIsOutOfRange(node.Right);
                        }
                        if (node.Left.Token.TokenType != TokenType.Range)
                        {
                            ReportIfSecondIsOutOfRange(node.Left);
                        }
                        else if(node.Left.Token.TokenType == TokenType.Range && range == null)
                        {
                            Visit(node, node.Left as RangeNode, recurenceLevel + 1);
                        }
                        else
                        {
                            Visit(range);
                        }
                        break;
                    case Segment.Minutes:
                        if(recurenceLevel == 1)
                        {
                            ReportIfLessThanZero(node.Right, nameof(node.Right));
                            ReportIfMinuteIsOutOfRange(node.Right);
                        }
                        if (node.Left.Token.TokenType != TokenType.Range)
                        {
                            ReportIfMinuteIsOutOfRange(node.Left);
                        }
                        else if (node.Left.Token.TokenType == TokenType.Range && range == null)
                        {
                            Visit(node, node.Left as RangeNode, recurenceLevel + 1);
                        }
                        else
                        {
                            Visit(range);
                        }
                        break;
                    case Segment.Hours:
                        if (recurenceLevel == 1)
                        {
                            ReportIfLessThanZero(node.Right, nameof(node.Right));
                            ReportIfHourIsOutOfRange(node.Right);
                        }
                        if (node.Left.Token.TokenType != TokenType.Range)
                        {
                            ReportIfHourIsOutOfRange(node.Left);
                        }
                        else if (node.Left.Token.TokenType == TokenType.Range && range == null)
                        {
                            Visit(node, node.Left as RangeNode, recurenceLevel + 1);
                        }
                        else
                        {
                            Visit(range);
                        }
                        break;
                    case Segment.DayOfMonth:
                        if (recurenceLevel == 1)
                        {
                            ReportIfLessThanZero(node.Right, nameof(node.Right));
                            ReportIfDayOfMonthIsOutOfRange(node.Right);
                        }
                        if (node.Left.Token.TokenType != TokenType.Range)
                        {
                            ReportIfDayOfMonthIsOutOfRange(node.Left);
                        }
                        else if (node.Left.Token.TokenType == TokenType.Range && range == null)
                        {
                            Visit(node, node.Left as RangeNode, recurenceLevel + 1);
                        }
                        else
                        {
                            Visit(range);
                        }
                        break;
                    case Segment.Month:
                        if (recurenceLevel == 1)
                        {
                            ReportIfLessThanZero(node.Right, nameof(node.Right));
                            ReportIfOutOfRange(1, 12, node.Right, "arg");
                        }
                        if (node.Left.Token.TokenType != TokenType.Range)
                        {
                            ReportIfMonthIsOutOfRange(node.Left);
                        }
                        else if (node.Left.Token.TokenType == TokenType.Range && range == null)
                        {
                            Visit(node, node.Left as RangeNode, recurenceLevel + 1);
                        }
                        else
                        {
                            Visit(range);
                        }
                        break;
                    case Segment.DayOfWeek:
                        if (recurenceLevel == 1)
                        {
                            ReportIfLessThanZero(node.Right, nameof(node.Right));
                            ReportIfOutOfRange(1, 7, node.Right, "arg");
                        }
                        if (node.Left.Token.TokenType != TokenType.Range)
                        {
                            ReportIfDayOfWeekIsOutOfRange(node.Left);
                        }
                        else if (node.Left.Token.TokenType == TokenType.Range && range == null)
                        {
                            Visit(node, node.Left as RangeNode, recurenceLevel + 1);
                        }
                        else
                        {
                            Visit(range);
                        }
                        break;
                    case Segment.Year:
                        if (recurenceLevel == 1)
                        {
                            ReportIfLessThanZero(node.Right, nameof(node.Right));
                            ReportIfOutOfRange(1, node.Right, "arg");
                        }
                        if (node.Left.Token.TokenType != TokenType.Range)
                        {
                            ReportIfYearIsOutOfRange(node.Left);
                        }
                        else if (node.Left.Token.TokenType == TokenType.Range && range == null)
                        {
                            Visit(node, node.Left as RangeNode, recurenceLevel + 1);
                        }
                        else
                        {
                            Visit(range);
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
        {
            var siblings = TreeHelpers.Siblings(currentSegment, node);
            if(siblings.Any())
            {
                AddSemanticError(node.FullSpan, 
                    string.Format(Properties.Resources.CannotBeUsedInThisContext, node.Token.Value, Properties.Resources.DoNotMixValues), 
                    SemanticErrorKind.UnsupportedValue);
            }
        }

        public virtual void Visit(CommaNode node)
        { }

        public void Visit(MissingNode node)
        {
            AddSyntaxError(node.FullSpan, string.Format(Properties.Resources.MissingValue, segment), SyntaxErrorKind.MissingValue);
        }

        private void AddSyntaxError(TextSpan fullSpan, string v, SyntaxErrorKind missingValue)
        {
            errors.Add(new SyntaxError(fullSpan, segment, v, missingValue));
        }

        private bool ReportIfFieldValueOfUnsupportedType(SyntaxNode node, params TokenType[] supportedTypes)
        {
            if(!supportedTypes.Contains(node.Token.TokenType))
            {
                AddSemanticError(
                    node.FullSpan,
                    string.Format(Properties.Resources.UnsupportedFieldValue, node.ToString(), node.Token.TokenType, string.Join(",", supportedTypes)),
                    SemanticErrorKind.UnsupportedValue);
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
            AddSemanticError(
                node.FullSpan,
                string.Format(Properties.Resources.OutOfRange, node.Token.Value, segment, "0", "31"),
                SemanticErrorKind.ValueOutOfRange);
        }

        private void ReportIfDayOfWeekIsOutOfRange(SyntaxNode node)
        {
            if (!CronWordHelper.ContainsDayOfWeek(node.Token.Value))
            {
                AddSemanticError(
                    node.FullSpan,
                    string.Format(Properties.Resources.OutOfRange, node.Token.Value, node.Token.TokenType, "1/MON", "7/SUN"),
                    SemanticErrorKind.ValueOutOfRange);
            }
        }

        private void ReportIfHashNodeOutOfRange(HashNode node)
        {
            if (!CronWordHelper.ContainsDayOfWeek(node.Left.Token.Value))
            {
                AddSemanticError(
                    node.FullSpan,
                    string.Format(Properties.Resources.OutOfRange, node.Left.Token.Value, segment, "0", "31"),
                    SemanticErrorKind.ValueOutOfRange);
            }
            var weekOfMonth = int.Parse(node.Right.Token.Value);
            if (weekOfMonth < 1 || weekOfMonth > 4)
            {
                AddSemanticError(
                    node.FullSpan,
                    string.Format(Properties.Resources.OutOfRange, node.Left.Token.Value, segment, "1", "4"),
                    SemanticErrorKind.ValueOutOfRange);
            }
        }

        private void ReportIfHourIsOutOfRange(SyntaxNode node)
        {
            if (node.Token.TokenType == TokenType.Integer)
            {
                ReportIfOutOfRange(0, 23, node, "hour");
                return;
            }
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
        }

        private void ReportIfMonthIsOutOfRange(SyntaxNode node)
        {
            if (!CronWordHelper.ContainsMonth(node.Token.Value))
            {
                AddSemanticError(
                    node.FullSpan,
                    string.Format(Properties.Resources.OutOfRange, node.Token.Value, segment, "1/JAN", "12/DEC"),
                    SemanticErrorKind.ValueOutOfRange);
            }
        }

        private void ReportIfNodesCountMismatched(SyntaxNode[] items, SyntaxNode parent)
        {
            if (items.Count() != 2)
            {
                AddSemanticError(
                    items.Select(f => f.FullSpan).ToArray(),
                    string.Format(Properties.Resources.NodeCountMismatched, 2, items.Count()), SemanticErrorKind.CountMismatched);
            }
        }

        private void ReportIfOutOfRange(int minValue, SyntaxNode node, string argName)
        {
            var value = int.Parse(node.Token.Value);
            if (value < minValue)
            {
                AddSemanticError(
                    node.FullSpan,
                    string.Format(Properties.Resources.OutOfRangeMin, value, segment, minValue), SemanticErrorKind.ValueOutOfRange);
            }
        }

        private void ReportIfOutOfRange(int minValue, int maxValue, SyntaxNode node, string argName)
        {
            var value = int.Parse(node.Token.Value);
            if (value < minValue || value > maxValue)
            {
                AddSemanticError(
                    node.FullSpan,
                    string.Format(Properties.Resources.OutOfRange, value, segment, minValue, maxValue), 
                    SemanticErrorKind.ValueOutOfRange);
            }
        }

        private void ReportIfSecondIsOutOfRange(SyntaxNode node)
        {
            if (node.Token.TokenType == TokenType.Integer)
            {
                ReportIfOutOfRange(0, 59, node, "second");
                return;
            }
        }

        private void ReportIfYearIsOutOfRange(SyntaxNode node)
        {
            if (node.Token.TokenType == TokenType.Integer)
            {
                ReportIfOutOfRange(1970, 3000, node, "year");
                return;
            }
        }

        private void ReportIfLWNodeAmongOtherValues(SyntaxNode node)
        {
            ReportIfMoreThanOneDescendants(node);
        }

        private void ReportIfMoreThanOneDescendants(SyntaxNode node)
        {
            if (parent.Desecendants.Count() != 1)
            {
                AddSemanticError(
                    parent.Desecendants.Select(f => f.FullSpan).ToArray(),
                    string.Format(Properties.Resources.NodeCountMismatched, 1, parent.Desecendants.Count()),
                    SemanticErrorKind.CountMismatched);
            }
        }

        private void ReportIfWNodeAmongOtherValues(SyntaxNode node)
        {
            ReportIfMoreThanOneDescendants(node);
        }
    }
}
