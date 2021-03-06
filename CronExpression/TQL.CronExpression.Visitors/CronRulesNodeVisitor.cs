﻿using System;
using System.Collections.Generic;
using System.Linq;
using TQL.Core.Tokens;
using TQL.CronExpression.Parser;
using TQL.CronExpression.Parser.Enums;
using TQL.CronExpression.Parser.Exceptions;
using TQL.CronExpression.Parser.Extensions;
using TQL.CronExpression.Parser.Helpers;
using TQL.CronExpression.Parser.Nodes;
using TQL.CronExpression.Visitors.Exceptions;

namespace TQL.CronExpression.Visitors
{
    public class CronRulesNodeVisitor : INodeVisitor
    {
        protected readonly List<Exception> CriticalErrors;
        private readonly List<VisitationMessage> _errors;
        private readonly bool _reportWhenExpressionTooShort;

        private SegmentNode _currentSegment;
        private CronSyntaxNode _parent;

        private Segment _segment;
        private short _segmentsCount;

        public CronRulesNodeVisitor(bool reportWhenExpressionTooShort = true)
        {
            CriticalErrors = new List<Exception>();
            _errors = new List<VisitationMessage>();
            _segmentsCount = 0;
            this._reportWhenExpressionTooShort = reportWhenExpressionTooShort;
        }

        public virtual IEnumerable<VisitationMessage> Errors => E;

        public virtual bool IsValid => CriticalErrors.Count == 0 && _errors.Count == 0;

        public object TreeHelper { get; }

        protected IReadOnlyList<VisitationMessage> E
            => _errors.Concat(CriticalErrors.Select(f => new FatalVisitError(f))).ToArray();

        public virtual void Visit(SegmentNode node)
        {
            _parent = node;
            _currentSegment = node;
            _segment = node.Segment;
            _segmentsCount += 1;
        }

        public virtual void Visit(RangeNode node)
        {
            try
            {
                var items = node.Desecendants;
                ReportIfNodesCountMismatched(items, node);
                switch (_segment)
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
                        CheckRangeNode(node, ReportIfNumericRangesSwaped, ReportIfDayOfMonthIsOutOfRange,
                            TokenType.Integer);
                        break;
                    }
                    case Segment.Year:
                    {
                        CheckRangeNode(node, ReportIfNumericRangesSwaped, ReportIfYearIsOutOfRange, TokenType.Integer);
                        break;
                    }
                    case Segment.DayOfWeek:
                    {
                        CheckRangeNode(node, ReportIfDayOfWeekRangesSwaped, ReportIfDayOfWeekIsOutOfRange,
                            TokenType.Integer, TokenType.Name);
                        break;
                    }
                    case Segment.Month:
                    {
                        CheckRangeNode(node, ReportIfMonthRangesSwaped, ReportIfMonthIsOutOfRange, TokenType.Integer,
                            TokenType.Name);
                        break;
                    }
                    default:
                    {
                        throw new UnexpectedSegmentException(_segment);
                    }
                }
            }
            catch (BaseCronValidationException exc)
            {
                CriticalErrors.Add(new RangeNodeException(exc));
            }
        }

        public virtual void Visit(WordNode node)
        {
            try
            {
                switch (_segment)
                {
                    case Segment.Seconds:
                    case Segment.Hours:
                    case Segment.Minutes:
                    case Segment.DayOfMonth:
                    case Segment.Year:
                        AddSemanticError(
                            node.FullSpan,
                            string.Format(Properties.Resources.UnsupportedFieldValue, node.Token.Value,
                                node.Token.TokenType, string.Join(",", GetSupportedTypes(_segment))),
                            SemanticErrorKind.UnsupportedValue);
                        break;
                }
                switch (_segment)
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
                CriticalErrors.Add(exc);
            }
        }

        public virtual void Visit(QuestionMarkNode node)
        {
            try
            {
                switch (_segment)
                {
                    case Segment.Seconds:
                    case Segment.Minutes:
                    case Segment.Hours:
                    case Segment.Month:
                    case Segment.Year:
                        AddSemanticError(
                            node.FullSpan,
                            string.Format(Properties.Resources.UnsupportedFieldValue, node.Token.Value,
                                node.Token.TokenType, string.Join(",", GetSupportedTypes(_segment))),
                            SemanticErrorKind.UnsupportedValue);
                        break;
                    case Segment.DayOfMonth:
                    case Segment.DayOfWeek:
                        var siblings = _currentSegment.Siblings(node);
                        if (siblings.Count() > 0)
                            AddSemanticError(
                                node.FullSpan,
                                string.Format(Properties.Resources.CannotBeUsedInThisContext, node.Token.Value,
                                    Properties.Resources.DoNotMixValues),
                                SemanticErrorKind.UnsupportedValue);
                        return;
                }
            }
            catch (BaseCronValidationException exc)
            {
                CriticalErrors.Add(exc);
            }
        }

        public virtual void Visit(LNode node)
        {
            try
            {
                switch (_segment)
                {
                    case Segment.DayOfMonth:
                        break;
                    case Segment.DayOfWeek:
                        ReportIfLNodeIsOutOfRange(node);
                        break;
                    default:
                        AddSemanticError(
                            node.FullSpan,
                            string.Format(Properties.Resources.UnsupportedFieldValue, node.Token.Value,
                                node.Token.TokenType, string.Join(",", GetSupportedTypes(_segment))),
                            SemanticErrorKind.UnsupportedValue);
                        break;
                }
            }
            catch (BaseCronValidationException exc)
            {
                CriticalErrors.Add(exc);
            }
        }

        public virtual void Visit(NumericPrecededLNode node)
        {
            try
            {
                switch (_segment)
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
                            string.Format(Properties.Resources.UnsupportedFieldValue, node.Token.Value,
                                node.Token.TokenType, string.Join(",", GetSupportedTypes(_segment))),
                            SemanticErrorKind.UnsupportedValue);
                        break;
                }
            }
            catch (BaseCronValidationException exc)
            {
                CriticalErrors.Add(exc);
            }
        }

        public virtual void Visit(NumericPrecededWNode node)
        {
            try
            {
                switch (_segment)
                {
                    case Segment.DayOfMonth:
                        var siblings = _currentSegment.Siblings(node);
                        if (siblings.Count() != 0)
                            AddSemanticError(node.FullSpan,
                                string.Format(Properties.Resources.CannotBeUsedInThisContext, node.Token.Value,
                                    Properties.Resources.DoNotMixValues),
                                SemanticErrorKind.UnsupportedValue);
                        break;
                    default:
                        AddSemanticError(node.FullSpan,
                            string.Format(Properties.Resources.UnsupportedFieldValue, node.Token.Value,
                                node.Token.TokenType, string.Join(",", GetSupportedTypes(_segment))),
                            SemanticErrorKind.UnsupportedValue);
                        break;
                }
            }
            catch (BaseCronValidationException exc)
            {
                CriticalErrors.Add(exc);
            }
        }

        public virtual void Visit(HashNode node)
        {
            try
            {
                switch (_segment)
                {
                    case Segment.DayOfWeek:
                        ReportIfHashNodeOutOfRange(node);
                        return;
                }
                AddSemanticError(
                    node.FullSpan,
                    string.Format(Properties.Resources.UnsupportedFieldValue, node.Token.Value, node.Token.TokenType,
                        string.Join(",", GetSupportedTypes(_segment))),
                    SemanticErrorKind.UnsupportedValue);
            }
            catch (BaseCronValidationException exc)
            {
                CriticalErrors.Add(exc);
            }
        }

        public virtual void Visit(EndOfFileNode node)
        {
            _segmentsCount += 1;
            try
            {
                if (_reportWhenExpressionTooShort && _segmentsCount != 8)
                    _errors.Add(new SyntaxError(node.FullSpan, _segment, Properties.Resources.UnexpectedEndOfFile,
                        SyntaxErrorKind.MissingValue));
            }
            catch (BaseCronValidationException exc)
            {
                CriticalErrors.Add(exc);
            }
        }

        public virtual void Visit(WNode node)
        {
            try
            {
                switch (_segment)
                {
                    case Segment.DayOfMonth:
                        var siblings = _currentSegment.Siblings(node);
                        if (siblings.Count() != 0)
                            AddSemanticError(node.FullSpan,
                                string.Format(Properties.Resources.CannotBeUsedInThisContext, node.Token.Value,
                                    Properties.Resources.DoNotMixValues),
                                SemanticErrorKind.UnsupportedValue);
                        ReportIfWNodeIsOutOfRange(node);
                        break;
                    default:
                        AddSemanticError(node.FullSpan,
                            string.Format(Properties.Resources.UnsupportedFieldValue, node.Token.Value,
                                node.Token.TokenType, string.Join(",", GetSupportedTypes(_segment))),
                            SemanticErrorKind.UnsupportedValue);
                        break;
                }
            }
            catch (BaseCronValidationException exc)
            {
                CriticalErrors.Add(exc);
            }
        }

        public virtual void Visit(LwNode node)
        {
            try
            {
                switch (_segment)
                {
                    case Segment.DayOfMonth:
                        ReportIfLwNodeAmongOtherValues(node);
                        break;
                    default:
                        AddSemanticError(
                            node.FullSpan,
                            string.Format(Properties.Resources.UnsupportedFieldValue, node.Token.Value,
                                node.Token.TokenType, string.Join(",", GetSupportedTypes(_segment))),
                            SemanticErrorKind.UnsupportedValue);
                        break;
                }
            }
            catch (BaseCronValidationException exc)
            {
                CriticalErrors.Add(exc);
            }
        }

        public virtual void Visit(NumberNode node)
        {
            try
            {
                switch (_segment)
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
                            string.Format(Properties.Resources.UnsupportedFieldValue, node.Token.Value,
                                node.Token.TokenType, string.Join(",", GetSupportedTypes(_segment))),
                            SemanticErrorKind.UnsupportedValue);
                        break;
                }
            }
            catch (BaseCronValidationException exc)
            {
                CriticalErrors.Add(exc);
            }
        }

        public virtual void Visit(IncrementByNode node)
        {
            Visit(node, null, 1);
        }

        public virtual void Visit(RootComponentNode node)
        {
        }

        public virtual void Visit(StarNode node)
        {
            var siblings = _currentSegment.Siblings(node);
            if (siblings.Any())
                AddSemanticError(node.FullSpan,
                    string.Format(Properties.Resources.CannotBeUsedInThisContext, node.Token.Value,
                        Properties.Resources.DoNotMixValues),
                    SemanticErrorKind.UnsupportedValue);
        }

        public virtual void Visit(CommaNode node)
        {
        }

        public void Visit(MissingNode node)
        {
            ReportMissingValue(node);
        }

        public virtual void Visit(IncrementByNode node, RangeNode range, int recurenceLevel)
        {
            try
            {
                switch (_segment)
                {
                    case Segment.Seconds:
                        if (recurenceLevel == 1)
                        {
                            ReportIfLessThanZero(node.Right, nameof(node.Right));
                            ReportIfSecondIsOutOfRange(node.Right);
                        }
                        if (node.Left.Token.TokenType != TokenType.Range)
                            ReportIfSecondIsOutOfRange(node.Left);
                        else if (node.Left.Token.TokenType == TokenType.Range && range == null)
                            Visit(node, node.Left as RangeNode, recurenceLevel + 1);
                        else
                            Visit(range);
                        break;
                    case Segment.Minutes:
                        if (recurenceLevel == 1)
                        {
                            ReportIfLessThanZero(node.Right, nameof(node.Right));
                            ReportIfMinuteIsOutOfRange(node.Right);
                        }
                        if (node.Left.Token.TokenType != TokenType.Range)
                            ReportIfMinuteIsOutOfRange(node.Left);
                        else if (node.Left.Token.TokenType == TokenType.Range && range == null)
                            Visit(node, node.Left as RangeNode, recurenceLevel + 1);
                        else
                            Visit(range);
                        break;
                    case Segment.Hours:
                        if (recurenceLevel == 1)
                        {
                            ReportIfLessThanZero(node.Right, nameof(node.Right));
                            ReportIfHourIsOutOfRange(node.Right);
                        }
                        if (node.Left.Token.TokenType != TokenType.Range)
                            ReportIfHourIsOutOfRange(node.Left);
                        else if (node.Left.Token.TokenType == TokenType.Range && range == null)
                            Visit(node, node.Left as RangeNode, recurenceLevel + 1);
                        else
                            Visit(range);
                        break;
                    case Segment.DayOfMonth:
                        if (recurenceLevel == 1)
                        {
                            ReportIfLessThanZero(node.Right, nameof(node.Right));
                            ReportIfDayOfMonthIsOutOfRange(node.Right);
                        }
                        if (node.Left.Token.TokenType != TokenType.Range)
                            ReportIfDayOfMonthIsOutOfRange(node.Left);
                        else if (node.Left.Token.TokenType == TokenType.Range && range == null)
                            Visit(node, node.Left as RangeNode, recurenceLevel + 1);
                        else
                            Visit(range);
                        break;
                    case Segment.Month:
                        if (recurenceLevel == 1)
                        {
                            ReportIfLessThanZero(node.Right, nameof(node.Right));
                            ReportIfOutOfRange(1, 12, node.Right, "arg");
                        }
                        if (node.Left.Token.TokenType != TokenType.Range)
                            ReportIfMonthIsOutOfRange(node.Left);
                        else if (node.Left.Token.TokenType == TokenType.Range && range == null)
                            Visit(node, node.Left as RangeNode, recurenceLevel + 1);
                        else
                            Visit(range);
                        break;
                    case Segment.DayOfWeek:
                        if (recurenceLevel == 1)
                        {
                            ReportIfLessThanZero(node.Right, nameof(node.Right));
                            ReportIfOutOfRange(1, 7, node.Right, "arg");
                        }
                        if (node.Left.Token.TokenType != TokenType.Range)
                            ReportIfDayOfWeekIsOutOfRange(node.Left);
                        else if (node.Left.Token.TokenType == TokenType.Range && range == null)
                            Visit(node, node.Left as RangeNode, recurenceLevel + 1);
                        else
                            Visit(range);
                        break;
                    case Segment.Year:
                        if (recurenceLevel == 1)
                        {
                            ReportIfLessThanZero(node.Right, nameof(node.Right));
                            ReportIfOutOfRange(1, node.Right, "arg");
                        }
                        if (node.Left.Token.TokenType != TokenType.Range)
                            ReportIfYearIsOutOfRange(node.Left);
                        else if (node.Left.Token.TokenType == TokenType.Range && range == null)
                            Visit(node, node.Left as RangeNode, recurenceLevel + 1);
                        else
                            Visit(range);
                        break;
                }
            }
            catch (BaseCronValidationException exc)
            {
                CriticalErrors.Add(exc);
            }
        }

        private static TokenType[] GetSupportedTypes(Segment segment)
        {
            switch (segment)
            {
                case Segment.DayOfMonth:
                    return new[]
                    {
                        TokenType.Integer, TokenType.Star, TokenType.Comma, TokenType.Range, TokenType.L, TokenType.W,
                        TokenType.QuestionMark
                    };
                case Segment.DayOfWeek:
                    return new[]
                    {
                        TokenType.Integer, TokenType.Star, TokenType.Comma, TokenType.Range, TokenType.L,
                        TokenType.Hash, TokenType.QuestionMark
                    };
                case Segment.Unknown:
                    return new TokenType[] {};
                default:
                    return new[] {TokenType.Integer, TokenType.Star, TokenType.Comma, TokenType.Range};
            }
        }

        private static bool IsComplexNode(CronSyntaxNode node)
            => node.Token.TokenType == TokenType.Hash || node.Token.TokenType == TokenType.Range;

        private void AddSemanticError(TextSpan span, string message, SemanticErrorKind kind)
        {
            AddSemanticError(new[] {span}, message, kind);
        }

        private void AddSemanticError(TextSpan[] spans, string message, SemanticErrorKind kind)
        {
            _errors.Add(new SemanticError(spans, _segment, message, kind));
        }

        private void AddSyntaxError(TextSpan fullSpan, string v, SyntaxErrorKind missingValue)
        {
            _errors.Add(new SyntaxError(fullSpan, _segment, v, missingValue));
        }

        private void CheckRangeNode(RangeNode node, Action<CronSyntaxNode[], RangeNode> action,
            Action<CronSyntaxNode> actionCheckOutOfRange, params TokenType[] types)
        {
            var hasUnsupportedLeftValue = false;
            var hasUnsupportedRightValue = false;
            var items = node.Desecendants;
            if (items[0].Token.TokenType == TokenType.Missing)
            {
                ReportMissingValue(items[0]);
                hasUnsupportedLeftValue = true;
            }
            else
            {
                hasUnsupportedLeftValue |= ReportIfFieldValueOfUnsupportedType(items[0], types);
            }
            if (items[1].Token.TokenType == TokenType.Missing)
            {
                ReportMissingValue(items[1]);
                hasUnsupportedRightValue = true;
            }
            else
            {
                hasUnsupportedRightValue |= ReportIfFieldValueOfUnsupportedType(items[1], types);
            }
            if (!hasUnsupportedLeftValue)
                actionCheckOutOfRange?.Invoke(items[0]);
            if (!hasUnsupportedRightValue)
                actionCheckOutOfRange?.Invoke(items[1]);
            if (!hasUnsupportedLeftValue && !hasUnsupportedRightValue)
                action?.Invoke(items, node);
        }

        private void ReportIfDayOfMonthIsOutOfRange(CronSyntaxNode node)
        {
            if (node.Token.TokenType == TokenType.Integer)
            {
                ReportIfOutOfRange(1, 31, node, "dayInMonth");
                return;
            }
            AddSemanticError(
                node.FullSpan,
                string.Format(Properties.Resources.OutOfRange, node.Token.Value, _segment, "1", "31"),
                SemanticErrorKind.ValueOutOfRange);
        }

        private void ReportIfWNodeIsOutOfRange(CronSyntaxNode node)
        {
            var number = 0;
            if (int.TryParse(node.Token.Value, out number) && number >= 1 && number <= 31)
                return;
            AddSemanticError(
                node.FullSpan,
                string.Format(Properties.Resources.OutOfRange, node.Token.Value, _segment, "1", "31"),
                SemanticErrorKind.ValueOutOfRange);
        }

        private void ReportIfLNodeIsOutOfRange(CronSyntaxNode node)
        {
            var number = 0;
            if (int.TryParse(node.Token.Value, out number) && number >= 1 && number <= 7)
                return;
            AddSemanticError(
                node.FullSpan,
                string.Format(Properties.Resources.OutOfRange, node.Token.Value, _segment, "2", "7"),
                SemanticErrorKind.ValueOutOfRange);
        }

        private void ReportIfDayOfWeekIsOutOfRange(CronSyntaxNode node)
        {
            if (!CronWordHelper.ContainsDayOfWeek(node.Token.Value))
                AddSemanticError(
                    node.FullSpan,
                    string.Format(Properties.Resources.OutOfRange, node.Token.Value, node.Token.TokenType, "1/MON",
                        "7/SUN"),
                    SemanticErrorKind.ValueOutOfRange);
        }

        private void ReportIfDayOfWeekRangesSwaped(CronSyntaxNode[] items, RangeNode node)
        {
            ReportIfRangesSwaped<int>(items, (a, b) =>
            {
                if (CronWordHelper.ContainsDayOfWeek(a) && CronWordHelper.ContainsDayOfWeek(b))
                {
                    var left = CronWordHelper.DayOfWeek(a);
                    var right = CronWordHelper.DayOfWeek(b);
                    if (left > right)
                        return true;
                }
                return false;
            });
        }

        private bool ReportIfFieldValueOfUnsupportedType(CronSyntaxNode node, params TokenType[] supportedTypes)
        {
            if (!supportedTypes.Contains(node.Token.TokenType))
            {
                ReportUnsupportedType(node);
                return true;
            }
            return false;
        }

        private void ReportIfHashNodeOutOfRange(HashNode node)
        {
            var isComplex = false;

            if (IsComplexNode(node.Left))
            {
                ReportUnsupportedType(node.Left, TokenType.Integer);
                isComplex = true;
            }

            if (IsComplexNode(node.Right))
            {
                ReportUnsupportedType(node.Right, TokenType.Integer);
                isComplex = true;
            }

            if (isComplex)
                return;

            if (!CronWordHelper.ContainsDayOfWeek(node.Left.Token.Value))
                if (node.Left.Token.TokenType != TokenType.Integer)
                    ReportUnsupportedType(node.Left, TokenType.Integer);
                else
                    ReportValueOutOfRange(node.Left, "1", "31");
            if (node.Right.Token.TokenType == TokenType.Integer)
            {
                var weekOfMonth = int.Parse(node.Right.Token.Value);
                if (weekOfMonth < 1 || weekOfMonth > 4)
                    ReportValueOutOfRange(node.Right, "1", "4");
            }
            else
            {
                ReportUnsupportedType(node.Right, TokenType.Integer);
            }
        }

        private void ReportIfHourIsOutOfRange(CronSyntaxNode node)
        {
            if (node.Token.TokenType == TokenType.Integer)
                ReportIfOutOfRange(0, 23, node, "hour");
        }

        private void ReportIfLessThanZero(CronSyntaxNode node, string argName)
        {
            ReportIfOutOfRange(0, node, argName);
        }

        private void ReportIfLwNodeAmongOtherValues(CronSyntaxNode node)
        {
            ReportIfMoreThanOneDescendants(node);
        }

        private void ReportIfMinuteIsOutOfRange(CronSyntaxNode node)
        {
            if (node.Token.TokenType == TokenType.Integer)
                ReportIfOutOfRange(0, 59, node, "minute");
        }

        private void ReportIfMonthIsOutOfRange(CronSyntaxNode node)
        {
            if (!CronWordHelper.ContainsMonth(node.Token.Value))
                AddSemanticError(
                    node.FullSpan,
                    string.Format(Properties.Resources.OutOfRange, node.Token, _segment, "1/JAN", "12/DEC"),
                    SemanticErrorKind.ValueOutOfRange);
        }

        private void ReportIfMonthRangesSwaped(CronSyntaxNode[] items, RangeNode node)
        {
            ReportIfRangesSwaped<int>(items, (a, b) =>
            {
                if (CronWordHelper.ContainsMonth(a) && CronWordHelper.ContainsMonth(b))
                {
                    var left = CronWordHelper.Month(a);
                    var right = CronWordHelper.Month(b);
                    if (left > right)
                        return true;
                }
                return false;
            });
        }

        private void ReportIfMoreThanOneDescendants(CronSyntaxNode node)
        {
            if (_parent.Desecendants.Count() != 1)
                AddSemanticError(
                    _parent.Desecendants.Select(f => f.FullSpan).ToArray(),
                    string.Format(Properties.Resources.NodeCountMismatched, 1, _parent.Desecendants.Count()),
                    SemanticErrorKind.CountMismatched);
        }

        private void ReportIfNodesCountMismatched(CronSyntaxNode[] items, CronSyntaxNode parent)
        {
            if (items.Count() != 2)
                AddSemanticError(
                    items.Select(f => f.FullSpan).ToArray(),
                    string.Format(Properties.Resources.NodeCountMismatched, 2, items.Count()),
                    SemanticErrorKind.CountMismatched);
        }

        private void ReportIfNumericRangesSwaped(CronSyntaxNode[] items, RangeNode node)
        {
            ReportIfRangesSwaped<int>(items, (a, b) =>
            {
                var left = int.Parse(a);
                var right = int.Parse(b);
                if (left > right)
                    return true;
                return false;
            });
        }

        private void ReportIfOutOfRange(int minValue, CronSyntaxNode node, string argName)
        {
            var value = int.Parse(node.Token.Value);
            if (value < minValue)
                AddSemanticError(
                    node.FullSpan,
                    string.Format(Properties.Resources.OutOfRangeMin, value, _segment, minValue),
                    SemanticErrorKind.ValueOutOfRange);
        }

        private void ReportIfOutOfRange(int minValue, int maxValue, CronSyntaxNode node, string argName)
        {
            var value = int.Parse(node.Token.Value);
            if (value < minValue || value > maxValue)
                AddSemanticError(
                    node.FullSpan,
                    string.Format(Properties.Resources.OutOfRange, node.Token.Value, _segment, minValue, maxValue),
                    SemanticErrorKind.ValueOutOfRange);
        }

        private void ReportIfRangesSwaped<T>(CronSyntaxNode[] items, Func<string, string, bool> compareAction)
        {
            if (items[0].Token.TokenType != TokenType.Integer && items[0].Token.TokenType != TokenType.Name)
                return;
            if (compareAction?.Invoke(items[0].Token.Value, items[1].Token.Value) ?? default(bool))
                AddSemanticError(
                    items.Select(f => f.FullSpan).ToArray(),
                    string.Format(Properties.Resources.RangeValueSwapped, items[0].Token.Value, items[1].Token.Value),
                    SemanticErrorKind.SwappedValue);
        }

        private void ReportIfSecondIsOutOfRange(CronSyntaxNode node)
        {
            if (node.Token.TokenType == TokenType.Integer)
                ReportIfOutOfRange(0, 59, node, "second");
        }

        private void ReportIfWNodeAmongOtherValues(CronSyntaxNode node)
        {
            ReportIfMoreThanOneDescendants(node);
        }

        private void ReportIfYearIsOutOfRange(CronSyntaxNode node)
        {
            if (node.Token.TokenType == TokenType.Integer)
                ReportIfOutOfRange(1970, 3000, node, "year");
        }

        private void ReportMissingValue(CronSyntaxNode node)
        {
            AddSyntaxError(node.FullSpan, string.Format(Properties.Resources.MissingValue, _segment),
                SyntaxErrorKind.MissingValue);
        }

        private void ReportUnsupportedType(CronSyntaxNode node, params TokenType[] supportedTypes)
        {
            AddSemanticError(
                node.FullSpan,
                string.Format(Properties.Resources.UnsupportedFieldValue, node, node.Token.TokenType,
                    string.Join(",", supportedTypes)),
                SemanticErrorKind.UnsupportedValue);
        }

        private void ReportValueOutOfRange(CronSyntaxNode node, string minValue, string maxValue)
        {
            AddSemanticError(
                node.FullSpan,
                string.Format(Properties.Resources.OutOfRange, node.Token.Value, _segment, minValue, maxValue),
                SemanticErrorKind.ValueOutOfRange);
        }
    }
}