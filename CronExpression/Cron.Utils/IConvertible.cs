namespace Cron.Utils
{
    public interface IConvertible<Input, Output>
    {
        Output Convert(Input input);
    }
}
