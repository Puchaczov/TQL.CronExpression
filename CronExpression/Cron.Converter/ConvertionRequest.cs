namespace TQL.CronExpression.Converter
{
    public class ConvertionRequest
    {

        private readonly string input;
        private readonly ConvertionOptions options;

        public ConvertionRequest(string input, CronMode mode)
            : this(input, ChoseOptionBasedOnMode(mode))
        {
            this.input = input;
        }

        public ConvertionRequest(string input, ConvertionOptions options)
        {
            this.input = input;
            this.options = options;
        }

        public enum CronMode
        {
            StandardDefinition,
            ModernDefinition
        }

        public string Input => this.input;

        public ConvertionOptions Options => this.options;

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
