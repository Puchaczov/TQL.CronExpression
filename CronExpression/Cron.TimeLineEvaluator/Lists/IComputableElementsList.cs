using System.Collections.Generic;

namespace TQL.CronExpression.Extensions.TimelineEvaluator.List
{
    public interface IComputableElementsList<T> : IEnumerable<T>
    {
        T this[int index] { get; set; }
        int Count { get; }
        T Element(int index);
        void Add(IComputableElementsList<T> list);
    }
}
