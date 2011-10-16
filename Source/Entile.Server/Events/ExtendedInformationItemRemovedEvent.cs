using System;

namespace Entile.Server.Events
{
    [Serializable]
    public class ExtendedInformationItemRemovedEvent : EventBase
    {
        public readonly string Key;

        public ExtendedInformationItemRemovedEvent(string key)
        {
            Key = key;
        }
    }
}