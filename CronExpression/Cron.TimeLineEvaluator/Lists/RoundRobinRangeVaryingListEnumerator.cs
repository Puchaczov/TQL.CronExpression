using System.Collections;
using System.Collections.Generic;

namespace Cron.Extensions.TimelineEvaluator.List
{
    internal class RoundRobinRangeVaryingListEnumerator<T> : IEnumerator<T>
    {
        private int index = -1;
        private readonly RoundRobinRangeVaryingList<T> roundRobinRangeVaryingList;

        public RoundRobinRangeVaryingListEnumerator(RoundRobinRangeVaryingList<T> roundRobinRangeVaryingList)
        {
            this.roundRobinRangeVaryingList = roundRobinRangeVaryingList;
        }

        public T Current => roundRobinRangeVaryingList.Element(index);

        object IEnumerator.Current => roundRobinRangeVaryingList.Element(index);

        public void Dispose()
        { }

        public bool MoveNext()
        {
            if (index + 1 < roundRobinRangeVaryingList.Count)
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