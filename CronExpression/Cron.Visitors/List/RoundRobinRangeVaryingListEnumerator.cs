using System;
using System.Collections;
using System.Collections.Generic;

namespace Cron.Parser.List
{
    internal class RoundRobinRangeVaryingListEnumerator<T> : IEnumerator<T>
    {
        private int index = -1;
        private readonly RoundRobinRangeVaryingList<T> roundRobinRangeVaryingList;

        public RoundRobinRangeVaryingListEnumerator(RoundRobinRangeVaryingList<T> roundRobinRangeVaryingList)
        {
            this.roundRobinRangeVaryingList = roundRobinRangeVaryingList;
        }

        public T Current
        {
            get
            {
                return roundRobinRangeVaryingList.Element(index);
            }
        }

        object IEnumerator.Current
        {
            get
            {
                return roundRobinRangeVaryingList.Element(index);
            }
        }

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