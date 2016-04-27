using System;
using System.Collections.Generic;

namespace Cron.Extensions.TimelineEvaluator.List
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
