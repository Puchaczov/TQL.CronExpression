using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cron.Extensions.TimelineEvaluator.List
{
    public class VirtualListEnumerator<T> : IEnumerator<T>
    {
        private int index;
        private readonly IComputableElementsEnumerable<T> list;

        public VirtualListEnumerator(IComputableElementsEnumerable<T> list)
        {
            this.list = list;
            this.index = -1;
        }

        public T Current => list.Element(index);

        object IEnumerator.Current => Current;

        public void Dispose()
        {
            Dispose(true);
        }

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

        #region IDisposable Support
        private bool disposedValue;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {

                }
                disposedValue = true;
            }
        }
        #endregion
    }
}
