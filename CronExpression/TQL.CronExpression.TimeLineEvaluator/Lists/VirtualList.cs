using System;
using System.Collections;
using System.Collections.Generic;
using TQL.CronExpression.Extensions.TimelineEvaluator.Lists.ComputableLists;

namespace TQL.CronExpression.Extensions.TimelineEvaluator.List
{
    public class VirtualList<T> : IComputableElementsList<T>, IEnumerable<T>
    {
        protected readonly IList<IComputableElementsList<T>> sources;

        public VirtualList()
        {
            this.sources = new List<IComputableElementsList<T>>();
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

        public virtual void Add(IComputableElementsList<T> list)
        {
            sources.Add(list);
        }

        public virtual T Element(int index)
        {
            var i = 0;
            var f = -1;
            var nidx = index;
            for(var j = sources.Count; i < j && nidx >= 0; ++i, ++f)
            {
                nidx -= sources[i].Count;
            }
            nidx += 1;
            if(nidx > 0)
            {
                throw new IndexOutOfRangeException(nameof(index));
            }
            var cnt = sources[f].Count;
            return sources[f][cnt - 1 + nidx];
        }

        public virtual IEnumerator<T> GetEnumerator() => new ComputableElementsEnumerator<T>(this);

        IEnumerator IEnumerable.GetEnumerator() => new ComputableElementsEnumerator<T>(this);
    }
}
