using Cron.Utils.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cron.Filters
{
    public class Trim : FilterBase<string>
    {
        protected override string Process(string input)
        {
            return input.Trim();
        }
    }
}
