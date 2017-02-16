using System.Collections.Generic;

namespace TQL.CronExpression.TimelineEvaluator.Lists
{
    public class RoundRobinRangeVaryingList<T> : RangeVaryingList<T>
    {
        private int _index;

        public virtual T Current => Element();
        public event OverflowedEventHandler Overflowed;

        public T Element() => base.Element(_index);

        public override IEnumerator<T> GetEnumerator() => new RoundRobinRangeVaryingListEnumerator<T>(this);

        public virtual void Next()
        {
            if (_index + 1 >= Count)
            {
                Overflowed?.Invoke(this, null);
                _index = 0;
                return;
            }
            _index += 1;
        }

        public void Overflow()
        {
            Overflowed?.Invoke(this, null);
            _index = 0;
        }

        public void Reset()
        {
            _index = 0;
        }

        public virtual bool WillOverflow() => _index + 1 >= Count;
    }
}