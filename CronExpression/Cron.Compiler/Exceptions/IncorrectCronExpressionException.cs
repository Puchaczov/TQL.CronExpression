using Cron.Visitors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cron.Compiler.Exceptions
{
    public class IncorrectCronExpressionException : Exception
    {
        private readonly IEnumerable<CompilationMessage> messages = new CompilationMessage[0];

        public IncorrectCronExpressionException(CompilationMessage message, params CompilationMessage[] messages)
            : this(messages.Concat(new CompilationMessage[] { message }).ToArray())
        { }

        public IncorrectCronExpressionException(params CompilationMessage[] messages)
        {
            this.messages = messages;
        }

        public CompilationMessage[] Messages => messages.ToArray();

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
