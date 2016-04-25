using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cron.Parser.List
{
    public class PersistentList<T> : List<T>, IComputableElementsEnumerable<T>
    {
        public PersistentList(IEnumerable<T> enumerable)
        {
            this.AddRange(enumerable);
        }

        public void Add(IComputableElementsEnumerable<T> list)
        {
            throw new NotImplementedException();
        }

        public T Element(int index) => this[index];
    }
}
