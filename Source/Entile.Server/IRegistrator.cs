

using System;

namespace Entile.Server
{
    public interface IRegistrator
    {
        void Register(Guid uniqueId, string notificationChannel);
        void Unregister(Guid uniqueId);
    }
}