using Cron.Visitors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cron.Exceptions
{
    public class IncorrectCronExpressionException : Exception
    {
        private readonly IEnumerable<VisitationMessage> messages = new VisitationMessage[0];

        public IncorrectCronExpressionException(VisitationMessage message, params VisitationMessage[] messages)
            : this(messages.Concat(new VisitationMessage[] { message }).ToArray())
        { }

        public IncorrectCronExpressionException(params VisitationMessage[] messages)
        {
            this.messages = messages;
        }

        public VisitationMessage[] Messages => messages.ToArray();

        public override string ToString()
        {
            var builder = new StringBuilder();
            var count = messages.Count();
            for(int i = 0; i < count - 1; ++i)
            {
                builder.Append(messages.ElementAt(i).ToString());
                builder.Append(Environment.NewLine);
            }
            if(count > 0)
            {
                builder.Append(messages.Last());
            }
            return builder.ToString();
        }
    }
}
