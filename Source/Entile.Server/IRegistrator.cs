

namespace Entile.Server
{
    public interface IRegistrator
    {
        void Register(string uniqueId, string notificationChannel);
        void Unregister(string uniqueId);
        void SetExtendedInformation(string uniqueId, string key, string value);
        void RemoveExtendedInformation(string uniqueId, string key);
        void RemoveAllExtendedInformation(string uniqueId);
    }
}