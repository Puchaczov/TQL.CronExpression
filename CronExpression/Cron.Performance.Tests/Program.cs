using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cron.Performance.Tests
{
    class Program
    {
        static void Main(string[] args)
        {
            var watch = Stopwatch.StartNew();

            var timeline = new Cron.CronTimeline(true);
            var response = timeline.Convert(new ConvertionRequest("* * * * * * *", ConvertionRequest.CronMode.ModernDefinition));
            var evaluator = response.Output;
            watch.Stop();
        }
    }
}
