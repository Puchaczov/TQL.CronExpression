namespace TQL.CronExpression
{
    public interface IConverter<in TRequest, out TResponse>
    {
        TResponse Convert(TRequest request);
    }
}
