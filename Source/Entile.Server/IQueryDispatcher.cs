namespace Entile.Server
{
    public interface IQueryDispatcher
    {
        object Invoke(IMessage query);
    }
}