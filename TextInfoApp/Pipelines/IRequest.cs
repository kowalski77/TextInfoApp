namespace TextInfoApp.Pipelines
{
    public interface IRequest<in T, out TR>
    {
        TR Invoke(T input);
    }
}