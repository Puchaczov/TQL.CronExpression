namespace Cron.Compilation
{
    public class CompilationRequest
    {

        private readonly string input;
        private readonly CompilationOptions options;

        public CompilationRequest(string input, CronMode mode)
            : this(input, ChoseOptionBasedOnMode(mode))
        {
            this.input = input;
        }

        public CompilationRequest(string input, CompilationOptions options)
        {
            this.input = input;
            this.options = options;
        }

        public string Input => this.input;

        public CompilationOptions Options => this.options;
        public class CompilationOptions
        {
            public bool ProduceEndOfFileNode { get; set; }
            public bool ProduceYearIfMissing { get; set; }
            public bool ProduceSecondsIfMissing { get; set; }
        }

        public enum CronMode
        {
            StandardDefinition,
            ModernDefinition
        }

        private static CompilationOptions ChoseOptionBasedOnMode(CronMode mode)
        {
            CompilationOptions option = null;
            switch (mode)
            {
                case CronMode.StandardDefinition:
                    option = new CompilationOptions
                    {
                        ProduceSecondsIfMissing = true,
                        ProduceYearIfMissing = true,
                        ProduceEndOfFileNode = true
                    };
                    break;
                default:
                    option = new CompilationOptions
                    {
                        ProduceSecondsIfMissing = false,
                        ProduceYearIfMissing = false,
                        ProduceEndOfFileNode = true
                    };
                    break;
            }
            return option;
        }
    }
}
