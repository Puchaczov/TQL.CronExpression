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
                        throw new UnexpectedWordNodeAtSegment(node.Token, segment);
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
                            break;
                        }
                        else if (node.Token.TokenType == TokenType.Name)
                        {
                            break;
                        }
                        throw new UnsupportedValueException(node.Token);
                    case Segment.DayOfWeek:
                        if (node.Token.TokenType == TokenType.Integer)
                        {
                            break;
                        }
                        if (node.Token.TokenType == TokenType.Name)
                        {
                            if (!CronWordHelper.ContainsDayOfWeek(node.Token.Value))
                            {
                                throw new UnproperDayOfWeekException(node.Token);
                            }
                            break;
                        }
                        throw new UnsupportedValueException(node.Token);
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
            try
            {
                if(segmentsCount < 6)
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
                        break;
                    default:
                        if (node.Right.Token.TokenType != TokenType.Integer && node.Right.Token.TokenType != TokenType.Name)
                        {
                            throw new UnsupportedValueException(node.Right.Token);
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
    }
}
