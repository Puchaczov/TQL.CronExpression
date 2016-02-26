using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cron.Parser.List
{
    public class VirtualListEnumerator<T> : IEnumerator<T>
    {
        private IVirtualList<T> list;
        private int index;

        public VirtualListEnumerator(IVirtualList<T> list)
        {
            this.list = list;
            this.index = -1;
        }

        public T Current
        {
            get
            {
                return list.Element(index);
            }
        }

        object IEnumerator.Current
        {
            get
            {
                return Current;
            }
        }

        public void Dispose()
        { }

        public bool MoveNext()
        {
            if(index + 1 < list.Count)
            {
                index += 1;
                return true;
            }
            return false;
        }

        public void Reset()
        {
            index = -1;
        }
    }
}
