

namespace Entile.Server
{
    public interface IRegistrator
    {
        void Register(string uniqueId, string notificationChannel);
        void Unregister(string uniqueId);
    }
}