namespace Cron.Common.Converters
{
    public interface IConvertible<TIn, TOut>
    {
        TOut Convert(TIn input);
    }
}
