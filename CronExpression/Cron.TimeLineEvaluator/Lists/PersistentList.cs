using System;
using System.Collections.Generic;

namespace Cron.Extensions.TimelineEvaluator.List
{
    public class PersistentList<T> : List<T>, IComputableElementsList<T>
    {
        public PersistentList(IEnumerable<T> enumerable)
        {
            this.AddRange(enumerable);
        }

        public void Add(IComputableElementsList<T> list)
        {
            throw new NotImplementedException();
        }

        public T Element(int index) => this[index];
    }
}
