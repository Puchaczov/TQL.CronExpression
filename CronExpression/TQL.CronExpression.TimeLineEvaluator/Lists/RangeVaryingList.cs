using System;

namespace TQL.CronExpression.TimelineEvaluator.Lists
{
    public class RangeVaryingList<T> : VirtualList<T>
    {
        private int _maxRange;
        private int _minRange;

        public override int Count => _maxRange - _minRange + 1;

        public override void Add(IComputableElementsList<T> list)
        {
            base.Add(list);
        }

        public override T Element(int index)
        {
            if (_minRange + index > _maxRange)
                throw new ArgumentOutOfRangeException(nameof(index));
            return base.Element(_minRange + index);
        }

        public void SetRange(int minRange, int maxRange)
        {
            var isEmpty = minRange == 0 && maxRange == -1;
            if (minRange < 0 || minRange >= Count && minRange > 0)
                throw new ArgumentOutOfRangeException(nameof(minRange));
            if (minRange > maxRange && !isEmpty)
                throw new ArgumentOutOfRangeException(nameof(minRange));
            if (maxRange < 0 && !isEmpty)
                throw new ArgumentOutOfRangeException(nameof(maxRange));
            if (maxRange < minRange && !isEmpty)
                throw new ArgumentOutOfRangeException(nameof(maxRange));

            this._minRange = minRange;
            this._maxRange = Math.Min(base.Count - 1, maxRange);
        }
    }
}