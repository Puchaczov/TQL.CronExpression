using System;
using System.Collections.Generic;

namespace TQL.CronExpression.TimelineEvaluator.Lists
{
    public delegate void OverflowedEventHandler(object sender, EventArgs e);

    public class RoundRobinRangeVaryingList<T> : RangeVaryingList<T>
    {
        private int index;

        public RoundRobinRangeVaryingList()
            : base()
        { }
        public event OverflowedEventHandler Overflowed;

        public virtual T Current => Element();

        public T Element() => base.Element(index);

        public override IEnumerator<T> GetEnumerator() => new RoundRobinRangeVaryingListEnumerator<T>(this);

        public virtual void Next()
        {
            if (index + 1 >= Count)
            {
                Overflowed?.Invoke(this, null);
                index = 0;
                return;
            }
            index += 1;
        }

        public void Overflow()
        {
            Overflowed?.Invoke(this, null);
            index = 0;
        }

        public void Reset()
        {
            this.index = 0;
        }

        public virtual bool WillOverflow() => index + 1 >= Count;
    }
}
