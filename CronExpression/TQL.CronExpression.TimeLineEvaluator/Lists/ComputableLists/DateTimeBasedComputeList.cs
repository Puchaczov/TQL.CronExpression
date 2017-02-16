using System;
using System.Collections;
using System.Collections.Generic;
using TQL.CronExpression.Parser.Utils;

namespace TQL.CronExpression.TimelineEvaluator.Lists.ComputableLists
{
    public abstract class DateTimeBasedComputeList : IComputableElementsList<int>
    {
        protected readonly IList<int> list;
        protected readonly Ref<DateTimeOffset> referenceTime;

        protected DateTimeBasedComputeList(Ref<DateTimeOffset> referenceTime, IList<int> list)
        {
            this.referenceTime = referenceTime;
            this.list = list;
        }

        public virtual int Count => list.Count;

        public int this[int index]
        {
            get
            {
                return Element(index);
            }

            set
            {
                throw new NotSupportedException();
            }
        }

        public void Add(IComputableElementsList<int> list)
        {
            throw new NotSupportedException();
        }

        public abstract int Element(int index);

        public IEnumerator<int> GetEnumerator() => new ComputableElementsEnumerator<int>(this);

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
