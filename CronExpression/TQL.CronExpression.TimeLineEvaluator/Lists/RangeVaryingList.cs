using System;

namespace TQL.CronExpression.TimelineEvaluator.List
{
    public class RangeVaryingList<T> : VirtualList<T>
    {
        private int maxRange;
        private int minRange;

        public override int Count => (maxRange - minRange) + 1;

        public override void Add(IComputableElementsList<T> list)
        {
            base.Add(list);
        }

        public override T Element(int index)
        {
            if (minRange + index > maxRange)
            {
                throw new ArgumentOutOfRangeException(nameof(index));
            }
            return base.Element(minRange + index);
        }

        public void SetRange(int minRange, int maxRange)
        {
            var isEmpty = (minRange == 0 && maxRange == -1);
            if (minRange < 0 || (minRange >= this.Count && minRange > 0))
            {
                throw new ArgumentOutOfRangeException(nameof(minRange));
            }
            if (minRange > maxRange && !isEmpty)
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
    }
}
