using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cron.Utils.Filters
{
    public class Pipeline<T> : IFilterChain<T>
    {
        private IFilter<T> root;

        public T Execute(T input) => root.Execute(input);

        public IFilterChain<T> Register(IFilter<T> filter)
        {
            if (root == null) root = filter;
            else root.Register(filter);
            return this;
        }
    }
}
