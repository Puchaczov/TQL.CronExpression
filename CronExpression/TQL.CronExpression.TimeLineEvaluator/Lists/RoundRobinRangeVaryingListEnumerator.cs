using System.Collections;
using System.Collections.Generic;

namespace TQL.CronExpression.TimelineEvaluator.Lists
{
    internal class RoundRobinRangeVaryingListEnumerator<T> : IEnumerator<T>
    {
        private readonly RoundRobinRangeVaryingList<T> _roundRobinRangeVaryingList;
        private int _index = -1;

        public RoundRobinRangeVaryingListEnumerator(RoundRobinRangeVaryingList<T> roundRobinRangeVaryingList)
        {
            this._roundRobinRangeVaryingList = roundRobinRangeVaryingList;
        }

        public T Current => _roundRobinRangeVaryingList.Element(_index);

        object IEnumerator.Current => _roundRobinRangeVaryingList.Element(_index);

        public void Dispose()
        {
        }

        public bool MoveNext()
        {
            if (_index + 1 < _roundRobinRangeVaryingList.Count)
            {
                _index += 1;
                return true;
            }
            return false;
        }

        public void Reset()
        {
            _index = -1;
        }
    }
}