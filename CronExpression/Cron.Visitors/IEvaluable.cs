using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cron.Visitors
{
    public interface IEvaluable<T>
    {
        T Evaluator { get; }
    }
}
