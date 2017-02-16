using TQL.Core.Converters;

namespace TQL.CronExpression
{
    public class ConvertionRequest : ConvertionRequestBase
    {
        public enum CronMode
        {
            StandardDefinition,
            ModernDefinition
        }

        public ConvertionRequest(string input, CronMode mode)
            : this(input, ChoseOptionBasedOnMode(mode))
        {
            Input = input;
        }

        public ConvertionRequest(string input, ConvertionOptions options)
        {
            Input = input;
            Options = options;
        }

        public string Input { get; }

        public ConvertionOptions Options { get; }

        private static ConvertionOptions ChoseOptionBasedOnMode(CronMode mode)
        {
            ConvertionOptions option = null;
            switch (mode)
            {
                case CronMode.StandardDefinition:
                    option = new ConvertionOptions
                    {
                        ProduceSecondsIfMissing = true,
                        ProduceYearIfMissing = true,
                        ProduceEndOfFileNode = true
                    };
                    break;
                default:
                    option = new ConvertionOptions
                    {
                        ProduceSecondsIfMissing = false,
                        ProduceYearIfMissing = false,
                        ProduceEndOfFileNode = true
                    };
                    break;
            }
            return option;
        }

        public class ConvertionOptions
        {
            public bool ProduceEndOfFileNode { get; set; }
            public bool ProduceSecondsIfMissing { get; set; }
            public bool ProduceYearIfMissing { get; set; }
        }
    }
}