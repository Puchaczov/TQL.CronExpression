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
        private Segment segment;
        private short segmentsCount;
        private List<Exception> errors;
        private SyntaxNode parent;

        public CronRulesNodeVisitor()
        {
            errors = new List<Exception>();
            segmentsCount = 0;
        }

        public virtual void Visit(SegmentNode node)
        {
            parent = node;
            segment = node.Segment;
            segmentsCount += 1;
        }

        public virtual IEnumerable<Exception> ValidationErrors
        {
            get
            {
                return errors;
            }
        }

        public virtual bool IsValid
        {
            get
            {
                return errors.Count == 0;
            }
        }

        public virtual void Visit(RangeNode node)
        {
            try
            {
                var items = node.Desecendants;
                ThrowIfNodesCountMismatched(items, node);
                switch (segment)
                {
                    case Segment.Seconds:
                        if (items[0].Token.TokenType != TokenType.Range)
                            ThrowIfSecondIsOutOfRange(items[0]);
                        ThrowIfSecondIsOutOfRange(items[1]);
                        break;
                    case Segment.Minutes:
                        if (items[0].Token.TokenType != TokenType.Range)
                            ThrowIfMinuteIsOutOfRange(items[0]);
                        ThrowIfMinuteIsOutOfRange(items[1]);
                        break;
                    case Segment.Hours:
                        if (items[0].Token.TokenType != TokenType.Range)
                            ThrowIfHourIsOutOfRange(items[0]);
                        ThrowIfHourIsOutOfRange(items[1]);
                        break;
                    case Segment.DayOfMonth:
                        if (items[0].Token.TokenType != TokenType.Range)
                            ThrowIfDayOfWeekIsOutOfRange(items[0]);
                        ThrowIfDayOfWeekIsOutOfRange(items[1]);
                        break;
                    case Segment.Year:
                        if (items[0].Token.TokenType != TokenType.Range)
                            ThrowIfYearIsOutOfRange(items[0]);
                        ThrowIfYearIsOutOfRange(items[1]);
                            break;
                    case Segment.DayOfWeek:
                        if (items[0].Token.TokenType != TokenType.Range)
                            ThrowIfDayOfWeekIsOutOfRange(items[0]);
                        ThrowIfDayOfWeekIsOutOfRange(items[1]);
                        break;
                    case Segment.Month:
                        if (items[0].Token.TokenType != TokenType.Range)
                            ThrowIfMonthIsOutOfRange(items[0]);
                        ThrowIfMonthIsOutOfRange(items[1]);
                        break;
                    default:
                        throw new UnexpectedSegmentException(segment);
                }
            }
            catch(BaseCronValidationException exc)
            {
                errors.Add(new RangeNodeException(exc));
            }
        }

        private void ThrowIfNodesCountMismatched(SyntaxNode[] items, SyntaxNode parent)
        {
            if(items.Count() != 2)
            {
                throw new MismatchedNodeItemsException(parent.Token);
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
                switch(segment)
                {
                    case Segment.Month:
                        ThrowIfMonthIsOutOfRange(node);
                        break;
                    case Segment.DayOfWeek:
                        ThrowIfDayOfWeekIsOutOfRange(node);
                        break;
                }
            }
            catch(BaseCronValidationException exc)
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
            catch(BaseCronValidationException exc)
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
            catch(BaseCronValidationException exc)
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
                        ThrowIfDayOfMonthIsOutOfRange(node);
                        break;
                    case Segment.DayOfWeek:
                        ThrowIfDayOfWeekIsOutOfRange(node);
                        break;
                    default:
                        throw new UnexpectedPrecededLNodeAtSegmentException();
                }
            }
            catch(BaseCronValidationException exc)
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
                        ThrowIfWNodeAmongOtherValues(node);
                        ThrowIfDayOfWeekIsOutOfRange(node);
                        return;
                }
                throw new UnexpectedPrecededWNodeAtSegmentException(node.Token, segment);
            }
            catch(BaseCronValidationException exc)
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
                        ThrowIfHashNodeOutOfRange(node);
                        break;
                    default:
                        throw new UnexpectedHashNodeAtSegment(node.Token, segment);
                }
            }
            catch(BaseCronValidationException exc)
            {
                errors.Add(exc);
            }
        }

        public virtual void Visit(EndOfFileNode node)
        {
            segmentsCount += 1;
            try
            {
                if(segmentsCount != 8)
                    throw new ExpressionTooShortException(node.Token, segment);
            }
            catch(BaseCronValidationException exc)
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
                        ThrowIfWNodeAmongOtherValues(node);
                        break;
                    default:
                        throw new UnexpectedWNodeAtSegment(node.Token, segment);
                }
            }
            catch(BaseCronValidationException exc)
            {
                errors.Add(exc);
            }
        }

        public virtual void Visit(LWNode node)
        {
            try
            {
                switch(segment)
                {
                    case Segment.DayOfMonth:
                        ThrowIfLWNodeAmongOtherValues(node);
                        break;
                    default:
                        throw new UnexpectedLWNodeAtSegment(node.Token, segment);
                }
            }
            catch(BaseCronValidationException exc)
            {
                errors.Add(exc);
            }
        }

        public virtual void Visit(NumberNode node)
        {
            try
            {
                switch(segment)
                {
                    case Segment.Seconds:
                        ThrowIfSecondIsOutOfRange(node);
                        break;
                    case Segment.Minutes:
                        ThrowIfMinuteIsOutOfRange(node);
                        break;
                    case Segment.Hours:
                        ThrowIfHourIsOutOfRange(node);
                        break;
                    case Segment.DayOfMonth:
                        ThrowIfDayOfMonthIsOutOfRange(node);
                        break;
                    case Segment.Month:
                        ThrowIfMonthIsOutOfRange(node);
                        break;
                    case Segment.DayOfWeek:
                        ThrowIfDayOfWeekIsOutOfRange(node);
                        break;
                    case Segment.Year:
                        ThrowIfYearIsOutOfRange(node);
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
                        ThrowIfLessThanZero(node.Right, nameof(node.Right));
                        if (node.Left.Token.TokenType != TokenType.Range)
                        {
                            ThrowIfSecondIsOutOfRange(node.Left);
                        }
                        break;
                    case Segment.Minutes:
                        ThrowIfLessThanZero(node.Right, nameof(node.Right));
                        if (node.Left.Token.TokenType != TokenType.Range)
                        {
                            ThrowIfMinuteIsOutOfRange(node.Left);
                        }
                        break;
                    case Segment.Hours:
                        ThrowIfLessThanZero(node.Right, nameof(node.Right));
                        if (node.Left.Token.TokenType != TokenType.Range)
                        {
                            ThrowIfHourIsOutOfRange(node.Left);
                        }
                        break;
                    case Segment.DayOfMonth:
                        ThrowIfLessThanZero(node.Right, nameof(node.Right));
                        if (node.Left.Token.TokenType != TokenType.Range)
                        {
                            ThrowIfDayOfMonthIsOutOfRange(node.Left);
                        }
                        break;
                    case Segment.Month:
                        ThrowIfLessThanZero(node.Right, nameof(node.Right));
                        if (node.Left.Token.TokenType != TokenType.Range)
                        {
                            ThrowIfMonthIsOutOfRange(node.Left);
                        }
                        break;
                    case Segment.DayOfWeek:
                        ThrowIfLessThanZero(node.Right, nameof(node.Right));
                        if (node.Left.Token.TokenType != TokenType.Range)
                        {
                            ThrowIfDayOfWeekIsOutOfRange(node.Left);
                        }
                        break;
                    case Segment.Year:
                        ThrowIfLessThanZero(node.Right, nameof(node.Right));
                        if(node.Left.Token.TokenType != TokenType.Range)
                        {
                            ThrowIfYearIsOutOfRange(node.Left);
                        }
                        break;
                }
            }
            catch(BaseCronValidationException exc)
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

        private void ThrowIfSecondIsOutOfRange(SyntaxNode node)
        {
            if(node.Token.TokenType == TokenType.Integer)
            {
                ThrowIfOutOfRange(0, 59, node, "second");
                return;
            }
            throw new UnsupportedValueException(node.Token);
        }

        private void ThrowIfMinuteIsOutOfRange(SyntaxNode node)
        {
            if(node.Token.TokenType == TokenType.Integer)
            {
                ThrowIfOutOfRange(0, 59, node, "minute");
                return;
            }
            throw new UnsupportedValueException(node.Token);
        }

        private void ThrowIfHourIsOutOfRange(SyntaxNode node)
        {
            if(node.Token.TokenType == TokenType.Integer)
            {
                ThrowIfOutOfRange(0, 23, node, "hour");
                return;
            }
            throw new UnsupportedValueException(node.Token);
        }

        private void ThrowIfMonthIsOutOfRange(SyntaxNode node)
        {
            if (!CronWordHelper.ContainsMonth(node.Token.Value))
            {
                throw new UnsupportedValueException(node.Token);
            }
        }

        private void ThrowIfDayOfWeekIsOutOfRange(SyntaxNode node)
        {
            if (!CronWordHelper.ContainsDayOfWeek(node.Token.Value))
            {
                throw new UnsupportedValueException(node.Token);
            }
        }

        private void ThrowIfDayOfMonthIsOutOfRange(SyntaxNode node)
        {
            if(node.Token.TokenType == TokenType.Integer)
            {
                ThrowIfOutOfRange(1, 32, node, "dayInMonth");
                return;
            }
            throw new UnsupportedValueException(node.Token);
        }

        private void ThrowIfYearIsOutOfRange(SyntaxNode node)
        {
            if(node.Token.TokenType == TokenType.Integer)
            {
                ThrowIfOutOfRange(1970, 3000, node, "year");
                return;
            }
            throw new UnsupportedValueException(node.Token);
        }

        private void ThrowIfHashNodeOutOfRange(HashNode node)
        {
            if (!CronWordHelper.ContainsDayOfWeek(node.Left.Token.Value))
            {
                throw new UnsupportedValueException(node.Left.Token);
            }
            var weekOfMonth = int.Parse(node.Right.Token.Value);
            if (weekOfMonth < 0 || weekOfMonth > 4)
            {
                throw new UnsupportedValueException(node.Right.Token);
            }
        }

        private void ThrowIfLessThanZero(SyntaxNode node, string argName)
        {
            ThrowIfOutOfRange(0, node, argName);
        }

        private void ThrowIfOutOfRange(int minValue, SyntaxNode node, string argName)
        {
            var value = int.Parse(node.Token.Value);
            if(value < minValue)
            {
                throw new UnsupportedValueException(node.Token);
            }
        }

        private void ThrowIfOutOfRange(int minValue, int maxValue, SyntaxNode node, string argName)
        {
            var value = int.Parse(node.Token.Value);
            if(value < minValue || value > maxValue)
            {
                throw new UnsupportedValueException(node.Token);
            }
        }

        private void ThrowIfWNodeAmongOtherValues(SyntaxNode node)
        {
            ThrowIfMoreThanOneDescendants(node);
        }

        private void ThrowIfLWNodeAmongOtherValues(SyntaxNode node)
        {
            ThrowIfMoreThanOneDescendants(node);
        }

        private void ThrowIfMoreThanOneDescendants(SyntaxNode node)
        {
            if (parent.Desecendants.Count() != 1)
            {
                throw new WNodeCannotBeMixedException(node.Token);
            }
        }
    }
}
