namespace Entile.Server
{
    public interface IMessageDispatcher
    {
        void Dispatch(IMessage message);
    }
}