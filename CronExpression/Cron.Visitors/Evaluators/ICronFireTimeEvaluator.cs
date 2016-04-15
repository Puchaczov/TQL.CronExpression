using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cron.Visitors.Evaluators
{
    public interface ICronFireTimeEvaluator
    {
        DateTime ReferenceTime { set; }
        DateTimeOffset OffsetReferenceTime { set; }
        DateTime? NextFire();
        DateTime? PreviousFire();
    }
}
