namespace Cron.Extensions.TimelineEvaluator
{
    public interface IEvaluable<T>
    {
        T Evaluator { get; }
    }
}
