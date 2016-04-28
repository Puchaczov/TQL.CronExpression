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
            var response = timeline.GetEvaluator(new ConvertionRequest(string.Format("0 0 * 1 * * {0}", DateTime.Now.Year), ConvertionRequest.CronMode.ModernDefinition));
            var evaluator = response.Output;
            while (evaluator.NextFire().HasValue)
            {

            }
            watch.Stop();
        }
    }
}
