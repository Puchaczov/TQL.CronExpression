using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cron.Extensions.TimelineEvaluator.List
{
    public interface IComputableElementsEnumerable<T> : IEnumerable<T>
    {
        T this[int index] { get; set; }
        int Count { get; }
        T Element(int index);
        void Add(IComputableElementsEnumerable<T> list);
    }
}
