using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Cron.Parser.List
{
    public class VirtualList<T> : IVirtualList<T>, IEnumerable<T>
    {
        protected readonly IList<IVirtualList<T>> sources;

        public VirtualList()
        {
            this.sources = new List<IVirtualList<T>>();
        }

        public virtual int Count
        {
            get
            {
                var count = 0;
                foreach (var list in sources)
                {
                    count += list.Count;
                }
                return count;
            }
        }

        public T this[int index]
        {
            get
            {
                return Element(index);
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public virtual void Add(IVirtualList<T> list)
        {
            sources.Add(list);
        }

        public virtual T Element(int index)
        {
            var i = -1;
            foreach (var l in sources)
            {
                foreach (var k in l)
                {
                    i += 1;
                    if (i == index)
                    {
                        return k;
                    }
                }
            }
            throw new IndexOutOfRangeException(nameof(index));
        }

        public virtual IEnumerator<T> GetEnumerator()
        {
            return new VirtualListEnumerator<T>(this);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return new VirtualListEnumerator<T>(this);
        }
    }
}
