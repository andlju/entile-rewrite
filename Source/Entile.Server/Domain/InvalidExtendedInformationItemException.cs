using System;
using System.Globalization;

namespace Entile.Server.Domain
{
    public class InvalidExtendedInformationItemException : Exception
    {
        public string UniqueId { get; private set; }
        public string Key { get; private set; }

        public InvalidExtendedInformationItemException(string uniqueId, string key) :
            base(string.Format(CultureInfo.InvariantCulture, "Client with uniqueId {0} has no extended information item with key {1}", uniqueId, key))
        {
            UniqueId = uniqueId;
            Key = key;
        }
    }
}