namespace Entile.Server
{
    public interface IQueryDispatcher
    {
        dynamic Invoke(IMessage query);
    }
}