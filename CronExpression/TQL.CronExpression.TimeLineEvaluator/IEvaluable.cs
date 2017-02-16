namespace TQL.CronExpression.TimelineEvaluator
{
    public interface IEvaluable<T>
    {
        T Evaluator { get; }
    }
}