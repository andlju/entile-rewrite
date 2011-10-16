using System;

namespace Entile.Server.Events
{
    [Serializable]
    public class ExtendedInformationItemSetEvent : EventBase
    {
        public readonly string Key;
        public readonly string Value;

        public ExtendedInformationItemSetEvent(string key, string value) 
        {
            Key = key;
            Value = value;
        }
    }
}