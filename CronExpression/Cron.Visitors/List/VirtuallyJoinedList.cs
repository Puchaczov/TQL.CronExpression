using System;
using System.Collections;
using System.Collections.Generic;

namespace Cron.Parser.List
{
    internal class VirtuallyJoinedList : IVirtualList<int>
    {
        private RoundRobinRangeVaryingList<int> dayOfMonths;
        private RoundRobinRangeVaryingList<int> dayOfWeeks;
        private Dictionary<int, int> correspondingKeys;
        private int index;

        public VirtuallyJoinedList(RoundRobinRangeVaryingList<int> dayOfMonths, RoundRobinRangeVaryingList<int> dayOfWeeks)
        {
            this.dayOfMonths = dayOfMonths;
            this.dayOfWeeks = dayOfWeeks;
            correspondingKeys = new Dictionary<int, int>();
            index = 0;
        }

        public void RebuildCorrespondingKeys()
        {
            var index = 0;
            correspondingKeys.Clear();
            for(int i = 0; i < dayOfMonths.Count; ++i)
            {
                for(int j = 0; j < dayOfWeeks.Count; ++j)
                {
                    if(dayOfMonths[i] == dayOfWeeks[j])
                    {
                        correspondingKeys.Add(index++, i);
                    }
                }
            }
        }

        public int Count
        {
            get
            {
                return correspondingKeys.Count;
            }
        }

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

        public int Element(int index)
        {
            return dayOfMonths[correspondingKeys[index]];
        }

        public IEnumerator<int> GetEnumerator()
        {
            return new VirtualListEnumerator<int>(this);
        }

        public void Next()
        {
            if(index + 1 >= Count)
            {
                dayOfMonths.Overflow();
                RebuildCorrespondingKeys();
                index = 0;
                return;
            }
            index += 1;
        }

        public int Current
        {
            get
            {
                return Element(index);
            }
        }

        public bool WillOverflow()
        {
            return index + 1 >= Count;
        }

        public void Add(IVirtualList<int> list)
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Overflow()
        {
            dayOfMonths.Overflow();
            RebuildCorrespondingKeys();
        }

        public void SetRange(int minRange, int maxRange)
        {
            dayOfMonths.SetRange(minRange, maxRange);
        }
    }
}