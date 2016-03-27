using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cron.Utils.Filters
{
    public interface IFilterChain<T>
    {
        T Execute(T input);

        IFilterChain<T> Register(IFilter<T> filter);
    }
}
