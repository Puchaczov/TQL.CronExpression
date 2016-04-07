using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cron.Parser.List
{
    public delegate void OverflowedEventHandler(object sender, EventArgs e);

    public class RoundRobinRangeVaryingList<T> : RangeVaryingList<T>
    {
        public event OverflowedEventHandler Overflowed;
        private int index;

        public RoundRobinRangeVaryingList()
            : base()
        { }

        public virtual void Next()
        {
            if(index + 1 >= Count)
            {
                Overflowed?.Invoke(this, null);
                index = 0;
                return;
            }
            index += 1;
        }

        public void Overflow()
        {
            Overflowed?.Invoke(this, null);
            index = 0;
        }

        public virtual bool WillOverflow()
        {
            return index + 1 >= Count;
        }

        public T Element()
        {
            return base.Element(index);
        }

        public virtual T Current
        {
            get
            {
                return Element();
            }
        }

        public override IEnumerator<T> GetEnumerator()
        {
            return new RoundRobinRangeVaryingListEnumerator<T>(this);
        }

        public void Reset()
        {
            this.index = 0;
        }
    }
}
