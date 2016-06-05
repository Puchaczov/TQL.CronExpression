using Cron.Common.Pipeline;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cron.Common.Filters
{
    public class LowerCase : FilterBase<string>
    {
        protected override string Process(string input) => input.ToLowerInvariant();
    }
}
