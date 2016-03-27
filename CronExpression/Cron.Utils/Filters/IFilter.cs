using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cron.Utils.Filters
{
    public interface IFilter<T>
    {
        void Register(IFilter<T> filter);
        T Execute(T input);
    }
}
