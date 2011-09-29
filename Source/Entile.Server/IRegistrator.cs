

using System;

namespace Entile.Server
{
    public interface IRegistrator
    {
        void Register(Guid uniqueId, string notificationChannel);
        void Unregister(Guid uniqueId);
        void SetExtendedInformation(Guid uniqueId, string key, string value);
        void RemoveExtendedInformation(Guid uniqueId, string key);
        void RemoveAllExtendedInformation(Guid uniqueId);
    }
}