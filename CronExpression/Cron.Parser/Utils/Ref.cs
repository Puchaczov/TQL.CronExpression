using System;

namespace TQL.CronExpression.Parser.Utils
{
    public class Ref<T>
    {
        private readonly Func<T> _get;
        private readonly Action<T> _set;

        public Ref(Func<T> @get, Action<T> @set)
        {
            _get = @get;
            _set = @set;
        }

        public T Value
        {
            get { return _get(); }
            set { _set(value); }
        }
    }
}
