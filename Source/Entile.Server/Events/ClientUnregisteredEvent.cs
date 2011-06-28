using System;

namespace Entile.Server.Events
{
    [Serializable]
    public class ClientUnregisteredEvent : EventBase
    {
        public ClientUnregisteredEvent()
        {
        }
    }
}