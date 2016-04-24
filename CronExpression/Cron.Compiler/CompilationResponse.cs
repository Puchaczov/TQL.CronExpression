using Cron.Visitors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cron.Compiler
{
    public class CompilationResponse<T>
    {
        public T Output { get; }
        public IReadOnlyCollection<CompilationMessage> Messages { get; }

        public CompilationResponse(T output)
            : this(new CompilationMessage[0])
        {
            Output = output;
        }

        public CompilationResponse(T output, params CompilationMessage[] messages)
            : this(messages)
        {
            Output = output;
        }

        public CompilationResponse(CompilationMessage message)
        {
            Messages = new List<CompilationMessage> { message };
            Output = default(T);
        }

        public CompilationResponse(IReadOnlyCollection<CompilationMessage> messages)
        {
            Messages = messages;
        }
    }
}
