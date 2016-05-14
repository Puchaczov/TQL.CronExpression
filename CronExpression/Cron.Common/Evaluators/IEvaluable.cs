namespace Cron.Common.Evaluators
{
    public interface IEvaluable<T>
    {
        T Evaluator { get; }
    }
}
