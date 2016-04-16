namespace Cron.Compilation
{
    public class CompilationRequest
    {

        private readonly string input;
        private readonly CompilationOptions options;

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
        }
    }
}
