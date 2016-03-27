using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cron.Compilation
{
    public class CompilationRequest
    {
        public class CompilationOptions
        {
            public bool ProduceYearIfMissing { get; set; }
            public bool ProduceEndOfFileNode { get; set; }
        }

        private string input;
        private CompilationOptions options;

        public CompilationRequest(string input, CompilationOptions options)
        {
            this.input = input;
            this.options = options;
        }

        public string Input
        {
            get
            {
                return this.input;
            }
        }

        public CompilationOptions Options
        {
            get
            {
                return this.options;
            }
        }
    }
}
