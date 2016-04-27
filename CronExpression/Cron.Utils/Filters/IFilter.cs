namespace Cron.Utils.Filters
{
    public interface IFilter<T>
    {
        void Register(IFilter<T> filter);
        T Execute(T input);
    }
}
