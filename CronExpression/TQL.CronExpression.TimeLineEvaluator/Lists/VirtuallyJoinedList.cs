using System;
using System.Collections;
using System.Collections.Generic;
using TQL.CronExpression.Extensions.TimelineEvaluator.Lists.ComputableLists;

namespace TQL.CronExpression.Extensions.TimelineEvaluator.List
{
    internal class VirtuallyJoinedList : IComputableElementsList<int>
    {
        private readonly Dictionary<int, int> correspondingKeys;
        private readonly RoundRobinRangeVaryingList<int> dayOfMonths;
        private readonly RoundRobinRangeVaryingList<int> dayOfWeeks;
        private int index;

        public VirtuallyJoinedList(RoundRobinRangeVaryingList<int> dayOfMonths, RoundRobinRangeVaryingList<int> dayOfWeeks)
        {
            this.dayOfMonths = dayOfMonths;
            this.dayOfWeeks = dayOfWeeks;
            correspondingKeys = new Dictionary<int, int>();
            index = 0;
        }

        public int Count => correspondingKeys.Count;

        public int Current => Element(index);

        public int this[int index]
        {
            get
            {
                return Element(index);
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public void Add(IComputableElementsList<int> list)
        {
            throw new NotImplementedException();
        }

        public int Element(int index) => dayOfMonths[correspondingKeys[index]];

        public IEnumerator<int> GetEnumerator() => new ComputableElementsEnumerator<int>(this);

        public void Next()
        {
            if (index + 1 >= Count)
            {
                dayOfMonths.Overflow();
                RebuildCorrespondingKeys();
                index = 0;
                return;
            }
            index += 1;
        }

        public void Overflow()
        {
            dayOfMonths.Overflow();
            RebuildCorrespondingKeys();
        }

        public void RebuildCorrespondingKeys()
        {
            var index = 0;
            correspondingKeys.Clear();
            for (int i = 0, k = dayOfMonths.Count; i < k; ++i)
            {
                for (int j = 0, f = dayOfWeeks.Count; j < f; ++j)
                {
                    if (dayOfMonths[i] == dayOfWeeks[j])
                    {
                        correspondingKeys.Add(index++, i);
                    }
                }
            }
        }

        public void Reset()
        {
            index = 0;
        }

        public void SetRange(int minRange, int maxRange)
        {
            dayOfMonths.SetRange(minRange, maxRange);
        }

        public bool WillOverflow() => index + 1 >= Count;

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}