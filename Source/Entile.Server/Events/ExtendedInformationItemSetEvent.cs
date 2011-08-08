using System;

namespace Entile.Server.Events
{
    [Serializable]
    public class ExtendedInformationItemSetEvent : EventBase
    {
        public string Key { get; private set; }
        public string Value { get; private set; }

        public ExtendedInformationItemSetEvent(string key, string value) 
        {
            Key = key;
            Value = value;
        }
    }
}