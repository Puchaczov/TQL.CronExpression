using System;
using System.Collections;
using System.Collections.Generic;

namespace TQL.CronExpression.TimelineEvaluator.Lists.ComputableLists
{
    public class ComputableElementsEnumerator<T> : IEnumerator<T>
    {
        private readonly IComputableElementsList<T> _list;
        private int _index;

        public ComputableElementsEnumerator(IComputableElementsList<T> list)
        {
            this._list = list;
            _index = -1;
        }

        public T Current => _list.Element(_index);

        object IEnumerator.Current => Current;

        public bool MoveNext()
        {
            if (_index + 1 < _list.Count)
            {
                _index += 1;
                return true;
            }
            return false;
        }

        public void Reset()
        {
            _index = -1;
        }

        #region IDisposable

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
        }

        #endregion
    }
}