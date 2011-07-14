using System;

namespace Entile.Server.Events
{
    [Serializable]
    public class ExtendedInformationItemRemovedEvent : EventBase
    {
        public string Key { get; private set; }

        public ExtendedInformationItemRemovedEvent(string key)
        {
            Key = key;
        }
    }
}