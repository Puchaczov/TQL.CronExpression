using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cron.Utils.Filters
{
    public abstract class FilterBase<T> : IFilter<T>
    {
        private IFilter<T> next;

        public T Execute(T input)
        {
            var val = Process(input);
            if (next != null) val = next.Execute(val);
            return val;
        }

        public void Register(IFilter<T> filter)
        {
            if(next == null)
            {
                next = filter;
            }
            else
            {
                next.Register(filter);
            }
        }

        protected abstract T Process(T input);
    }
}
