using System;
using System.Collections;
using System.Collections.Generic;
using TQL.CronExpression.Extensions.TimelineEvaluator.List;

namespace TQL.CronExpression.Extensions.TimelineEvaluator.Lists.ComputableLists
{
    public class ComputableElementsEnumerator<T> : IEnumerator<T>
    {
        private int index;
        private readonly IComputableElementsList<T> list;

        public ComputableElementsEnumerator(IComputableElementsList<T> list)
        {
            this.list = list;
            index = -1;
        }

        public T Current => list.Element(index);

        object IEnumerator.Current => Current;

        public bool MoveNext()
        {
            if (index + 1 < list.Count)
            {
                index += 1;
                return true;
            }
            return false;
        }

        public void Reset()
        {
            index = -1;
        }

        #region IDisposable
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        { }
        #endregion
    }
}
