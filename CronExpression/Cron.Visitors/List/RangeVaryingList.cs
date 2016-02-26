using System;

namespace Cron.Parser.List
{
    public class RangeVaryingList<T> : VirtualList<T>
    {
        private int minRange;
        private int maxRange;

        public void SetRange(int minRange, int maxRange)
        {
            bool isEmpty = (minRange == 0 && maxRange == -1);
            if (minRange < 0 || (minRange >= this.Count && minRange > 0))
            {
                throw new ArgumentOutOfRangeException(nameof(minRange));
            }
            if (minRange > maxRange && !isEmpty )
            {
                throw new ArgumentOutOfRangeException(nameof(minRange));
            }
            if (maxRange < 0 && !isEmpty)
            {
                throw new ArgumentOutOfRangeException(nameof(maxRange));
            }
            if (maxRange < minRange && !isEmpty)
            {
                throw new ArgumentOutOfRangeException(nameof(maxRange));
            }

            this.minRange = minRange;
            this.maxRange = Math.Min(base.Count - 1, maxRange);
        }

        public override void Add(IVirtualList<T> list)
        {
            base.Add(list);
        }

        public override T Element(int index)
        {
            if(minRange + index > maxRange)
            {
                throw new ArgumentOutOfRangeException(nameof(index));
            }
            return base.Element(minRange + index);
        }

        public override int Count
        {
            get
            {
                return (maxRange - minRange) + 1;
            }
        }
    }
}
