using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TQL.CronExpression.Visitors;

namespace TQL.CronExpression.Exceptions
{
    public class IncorrectCronExpressionException : Exception
    {
        private readonly IEnumerable<VisitationMessage> _messages = new VisitationMessage[0];

        public IncorrectCronExpressionException(VisitationMessage message, params VisitationMessage[] messages)
            : this(messages.Concat(new[] {message}).ToArray())
        {
        }

        public IncorrectCronExpressionException(params VisitationMessage[] messages)
        {
            this._messages = messages;
        }

        public VisitationMessage[] Messages => _messages.ToArray();

        public override string ToString()
        {
            var builder = new StringBuilder();
            var count = _messages.Count();
            for (var i = 0; i < count - 1; ++i)
            {
                builder.Append(_messages.ElementAt(i));
                builder.Append(Environment.NewLine);
            }
            if (count > 0)
                builder.Append(_messages.Last());
            return builder.ToString();
        }
    }
}