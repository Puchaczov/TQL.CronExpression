using System;
using System.Collections;
using System.Collections.Generic;
using TQL.CronExpression.TimelineEvaluator.Lists.ComputableLists;

namespace TQL.CronExpression.TimelineEvaluator.Lists
{
    internal class VirtuallyJoinedList : IComputableElementsList<int>
    {
        private readonly Dictionary<int, int> _correspondingKeys;
        private readonly RoundRobinRangeVaryingList<int> _dayOfMonths;
        private readonly RoundRobinRangeVaryingList<int> _dayOfWeeks;
        private int _index;

        public VirtuallyJoinedList(RoundRobinRangeVaryingList<int> dayOfMonths,
            RoundRobinRangeVaryingList<int> dayOfWeeks)
        {
            this._dayOfMonths = dayOfMonths;
            this._dayOfWeeks = dayOfWeeks;
            _correspondingKeys = new Dictionary<int, int>();
            _index = 0;
        }

        public int Current => Element(_index);

        public int Count => _correspondingKeys.Count;

        public int this[int index]
        {
            get { return Element(index); }

            set { throw new NotImplementedException(); }
        }

        public void Add(IComputableElementsList<int> list)
        {
            throw new NotImplementedException();
        }

        public int Element(int index) => _dayOfMonths[_correspondingKeys[index]];

        public IEnumerator<int> GetEnumerator() => new ComputableElementsEnumerator<int>(this);

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public void Next()
        {
            if (_index + 1 >= Count)
            {
                _dayOfMonths.Overflow();
                RebuildCorrespondingKeys();
                _index = 0;
                return;
            }
            _index += 1;
        }

        public void Overflow()
        {
            _dayOfMonths.Overflow();
            RebuildCorrespondingKeys();
        }

        public void RebuildCorrespondingKeys()
        {
            var index = 0;
            _correspondingKeys.Clear();
            for (int i = 0, k = _dayOfMonths.Count; i < k; ++i)
            for (int j = 0, f = _dayOfWeeks.Count; j < f; ++j)
                if (_dayOfMonths[i] == _dayOfWeeks[j])
                    _correspondingKeys.Add(index++, i);
        }

        public void Reset()
        {
            _index = 0;
        }

        public void SetRange(int minRange, int maxRange)
        {
            _dayOfMonths.SetRange(minRange, maxRange);
        }

        public bool WillOverflow() => _index + 1 >= Count;
    }
}