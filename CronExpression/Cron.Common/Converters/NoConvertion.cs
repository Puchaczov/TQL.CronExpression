namespace Cron.Common.Converters
{
    public class NoConvertion<T> : IConvertible<T, T>
    {
        public T Convert(T input) => input;
    }
}
