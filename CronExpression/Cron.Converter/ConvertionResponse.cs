using System.Collections.Generic;
using TQL.Core.Converters;
using TQL.CronExpression.Visitors;

namespace TQL.CronExpression.Converter
{
    public class ConvertionResponse<T> : ConvertionResponseBase<T>
    {

        public ConvertionResponse(T output)
            : this(new VisitationMessage[0])
        {
            Output = output;
        }

        public ConvertionResponse(T output, params VisitationMessage[] messages)
            : this(messages)
        {
            Output = output;
        }

        public ConvertionResponse(VisitationMessage message)
        {
            Messages = new List<VisitationMessage> { message };
            Output = default(T);
        }

        public ConvertionResponse(IReadOnlyCollection<VisitationMessage> messages)
        {
            Messages = messages;
        }

        public IReadOnlyCollection<VisitationMessage> Messages { get; }
        public T Output { get; }
    }
}
