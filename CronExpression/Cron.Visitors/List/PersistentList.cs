using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cron.Parser.List
{
    public class PersistentList<T> : List<T>, IVirtualList<T>
    {
        public PersistentList(IEnumerable<T> enumerable)
        {
            this.AddRange(enumerable);
        }

        public void Add(IVirtualList<T> list)
        {
            throw new NotImplementedException();
        }

        public T Element(int index)
        {
            return this[index];
        }
    }
}
