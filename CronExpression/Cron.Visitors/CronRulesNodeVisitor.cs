using System;
using System.Collections.Generic;
using Cron.Parser.Nodes;
using Cron.Parser.Enums;
using Cron.Parser.Exceptions;
using Cron.Parser.Extensions;
using Cron.Parser.Visitors;
using Cron.Visitors.Exceptions;

namespace Cron.Visitors
{
    public class CronRulesNodeVisitor : INodeVisitor
    {
        private Segment segment;
        private short segmentsCount;
        private List<Exception> errors;

        public CronRulesNodeVisitor()
        {
            errors = new List<Exception>();
            segmentsCount = 0;
        }

        public virtual void Visit(SegmentNode node)
        {
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
                var items = node.Items;
                switch (segment)
                {
                    case Segment.Seconds:
                    case Segment.Minutes:
                    case Segment.Hours:
                    case Segment.DayOfMonth:
                    case Segment.Year:
                        foreach (var item in items)
                        {
                            if (item.Token.TokenType != TokenType.Integer && item.Token.TokenType != TokenType.Range)
                            {
                                throw new UnsupportedValueException(item.Token);
                            }
                        }
                        break;
                    default:
                        foreach (var item in items)
                        {
                            switch (item.Token.TokenType)
                            {
                                case TokenType.Integer:
                                    break;
                                default:
                                    switch (segment)
                                    {
                                        case Segment.Month:
                                            if (!CronWordHelper.ContainsMonth(item.Token.Value))
                                            {
                                                throw new UnproperMonthNameException(item.Token);
                                            }
                                            break;
                                        case Segment.DayOfWeek:
                                            if (!CronWordHelper.ContainsDayOfWeek(item.Token.Value))
                                            {
                                                throw new UnproperDayOfWeekException(item.Token);
                                            }
                                            break;
                                    }
                                    break;
                            }
                        }
                        break;
                }
            }
            catch(BaseCronValidationException exc)
            {
                errors.Add(exc);
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
                        if(!CronWordHelper.ContainsMonth(node.Value))
                        {
                            throw new UnrecognizedMonthException(node.Token);
                        }
                        break;
                    case Segment.DayOfWeek:
                        if(!CronWordHelper.ContainsDayOfWeek(node.Value))
                        {
                            throw new UnrecognizedDayOfWeekException(node.Token);
                        }
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
                        throw new UnexpectedWNodeAtSegment(node.Token, segment);
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
                        if (node.Token.TokenType == TokenType.Integer)
                        {
                            ThrowIfDayOfWeekIsOutOfRange(node);
                            break;
                        }
                        else if (node.Token.TokenType == TokenType.Name)
                        {
                            ThrowIfDayOfWeekIsOutOfRange(node);
                            break;
                        }
                        throw new UnsupportedValueException(node.Token);
                    case Segment.DayOfWeek:
                        if (node.Token.TokenType == TokenType.Integer)
                        {
                            ThrowIfDayOfWeekIsOutOfRange(node);
                            break;
                        }
                        if (node.Token.TokenType == TokenType.Name)
                        {
                            ThrowIfDayOfWeekIsOutOfRange(node);
                            break;
                        }
                        throw new UnsupportedValueException(node.Token);
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
                    case Segment.DayOfWeek:
                        ThrowIfDayOfWeekIsOutOfRange(node);
                        return;
                }
                throw new UnexpectedPrecededWNodeAtSegmentException(segment);
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
                    case Segment.DayOfWeek:
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

        public virtual void Visit(SyntaxOperatorNode node)
        {
            throw new Exception("Unknown node exception");
        }

        public virtual void Visit(NumberNode node)
        {
            try
            {
                int.Parse(node.Value);
            }
            catch (Exception exc)
            {
                errors.Add(exc);
            }
        }

        public virtual void Visit(IncrementByNode node)
        {
            try
            {
                var items = node.Items;
                switch (segment)
                {
                    case Segment.Seconds:
                    case Segment.Minutes:
                    case Segment.Hours:
                    case Segment.DayOfMonth:
                    case Segment.Year:
                        if (node.Right.Token.TokenType != TokenType.Integer)
                        {
                            throw new UnsupportedValueException(node.Token);
                        }
                        if(node.Left.Token.TokenType != TokenType.Integer && node.Left.Token.TokenType != TokenType.Range)
                        {
                            throw new UnsupportedValueException(node.Token);
                        }
                        break;
                    default:
                        if (node.Right.Token.TokenType != TokenType.Integer && node.Right.Token.TokenType != TokenType.Name)
                        {
                            throw new UnsupportedValueException(node.Right.Token);
                        }
                        if (node.Left.Token.TokenType != TokenType.Integer && node.Left.Token.TokenType != TokenType.Name)
                        {
                            throw new UnsupportedValueException(node.Right.Token);
                        }
                        break;
                }
                switch (segment)
                {
                    case Segment.Seconds:
                        ThrowIfSecondIsOutOfRange(node.Right);
                        if(node.Left.Token.TokenType != TokenType.Range)
                        {
                            ThrowIfSecondIsOutOfRange(node.Left);
                        }
                        break;
                    case Segment.Minutes:
                        ThrowIfMinuteIsOutOfRange(node.Right);
                        if(node.Left.Token.TokenType != TokenType.Range)
                        {
                            ThrowIfMinuteIsOutOfRange(node.Left);
                        }
                        break;
                    case Segment.Hours:
                        ThrowIfHourIsOutOfRange(node.Right);
                        if (node.Left.Token.TokenType != TokenType.Range)
                        {
                            ThrowIfHourIsOutOfRange(node.Left);
                        }
                        break;
                    case Segment.DayOfMonth:
                        ThrowIfDayOfMonthIsOutOfRange(node.Right);
                        if (node.Left.Token.TokenType != TokenType.Range)
                        {
                            ThrowIfDayOfMonthIsOutOfRange(node.Left);
                        }
                        break;
                    case Segment.Month:
                        ThrowIfMonthIsOutOfRange(node.Right);
                        if (node.Left.Token.TokenType != TokenType.Range)
                        {
                            ThrowIfMonthIsOutOfRange(node.Left);
                        }
                        break;
                    case Segment.DayOfWeek:
                        ThrowIfDayOfWeekIsOutOfRange(node.Right);
                        if (node.Left.Token.TokenType != TokenType.Range)
                        {
                            ThrowIfDayOfWeekIsOutOfRange(node.Left);
                        }
                        break;
                    case Segment.Year:
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

        private void ThrowIfSecondIsOutOfRange(SyntaxOperatorNode node)
        {
            ThrowIfOutOfRange(0, 59, node, "second");
        }

        private void ThrowIfMinuteIsOutOfRange(SyntaxOperatorNode node)
        {
            ThrowIfOutOfRange(0, 59, node, "minute");
        }

        private void ThrowIfHourIsOutOfRange(SyntaxOperatorNode node)
        {
            ThrowIfOutOfRange(0, 23, node, "hour");
        }

        private void ThrowIfMonthIsOutOfRange(SyntaxOperatorNode node)
        {
            if (!CronWordHelper.ContainsMonth(node.Token.Value))
            {
                throw new UnsupportedValueException(node.Token);
            }
        }

        private void ThrowIfDayOfWeekIsOutOfRange(SyntaxOperatorNode node)
        {
            if (!CronWordHelper.ContainsDayOfWeek(node.Token.Value))
            {
                throw new UnsupportedValueException(node.Token);
            }
        }

        private void ThrowIfDayOfMonthIsOutOfRange(SyntaxOperatorNode node)
        {
            ThrowIfOutOfRange(1, 32, node, "dayInMonth");
        }

        private void ThrowIfHashNodeOutOfRange(HashNode node)
        {
            if (!CronWordHelper.ContainsDayOfWeek(node.Left.Value))
            {
                throw new UnsupportedValueException(node.Left);
            }
            var weekOfMonth = int.Parse(node.Right.Value);
            if (weekOfMonth < 0 || weekOfMonth > 4)
            {
                throw new UnsupportedValueException(node.Right);
            }
        }

        private void ThrowIfOutOfRange(int minValue, int maxValue, SyntaxOperatorNode node, string argName)
        {
            var value = int.Parse(node.Token.Value);
            if(value < minValue || value > maxValue)
            {
                throw new UnsupportedValueException(node.Token);
            }
        }
    }
}
