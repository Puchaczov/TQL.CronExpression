using System;
using System.Collections;
using System.Collections.Generic;
using TQL.CronExpression.TimelineEvaluator.Lists.ComputableLists;

namespace TQL.CronExpression.TimelineEvaluator.Lists
{
    public class VirtualList<T> : IComputableElementsList<T>, IEnumerable<T>
    {
        protected readonly IList<IComputableElementsList<T>> Sources;

        public VirtualList()
        {
            Sources = new List<IComputableElementsList<T>>();
        }

        public virtual int Count
        {
            get
            {
                var count = 0;
                foreach (var list in Sources)
                    count += list.Count;
                return count;
            }
        }

        public T this[int index]
        {
            get { return Element(index); }

            set { throw new NotImplementedException(); }
        }

        public virtual void Add(IComputableElementsList<T> list)
        {
            Sources.Add(list);
        }

        public virtual T Element(int index)
        {
            var i = 0;
            var f = -1;
            var nidx = index;
            for (var j = Sources.Count; i < j && nidx >= 0; ++i, ++f)
                nidx -= Sources[i].Count;
            nidx += 1;
            if (nidx > 0)
                throw new IndexOutOfRangeException(nameof(index));
            var cnt = Sources[f].Count;
            return Sources[f][cnt - 1 + nidx];
        }

        public virtual IEnumerator<T> GetEnumerator() => new ComputableElementsEnumerator<T>(this);

        IEnumerator IEnumerable.GetEnumerator() => new ComputableElementsEnumerator<T>(this);
    }
}