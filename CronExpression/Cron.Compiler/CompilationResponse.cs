using Cron.Visitors;
using System.Collections.Generic;

namespace Cron.Compiler
{
    public class CompilationResponse<T>
    {
        public T Output { get; }
        public IReadOnlyCollection<VisitationMessage> Messages { get; }

        public CompilationResponse(T output)
            : this(new VisitationMessage[0])
        {
            Output = output;
        }

        public CompilationResponse(T output, params VisitationMessage[] messages)
            : this(messages)
        {
            Output = output;
        }

        public CompilationResponse(VisitationMessage message)
        {
            Messages = new List<VisitationMessage> { message };
            Output = default(T);
        }

        public CompilationResponse(IReadOnlyCollection<VisitationMessage> messages)
        {
            Messages = messages;
        }
    }
}
