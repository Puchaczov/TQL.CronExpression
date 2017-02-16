using System;
using System.Collections;
using System.Collections.Generic;
using TQL.CronExpression.Parser.Utils;

namespace TQL.CronExpression.TimelineEvaluator.Lists.ComputableLists
{
    public abstract class DateTimeBasedComputeList : IComputableElementsList<int>
    {
        protected readonly IList<int> List;
        protected readonly Ref<DateTimeOffset> ReferenceTime;

        protected DateTimeBasedComputeList(Ref<DateTimeOffset> referenceTime, IList<int> list)
        {
            this.ReferenceTime = referenceTime;
            this.List = list;
        }

        public virtual int Count => List.Count;

        public int this[int index]
        {
            get { return Element(index); }

            set { throw new NotSupportedException(); }
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