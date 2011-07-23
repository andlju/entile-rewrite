using System;
using System.Globalization;

namespace Entile.Server.Domain
{
    public class ClientNotRegisteredException : Exception
    {
        public string UniqueId { get; private set; }

        public ClientNotRegisteredException(string uniqueId) :
            base(string.Format(CultureInfo.InvariantCulture, "Client with uniqueId {0} was not found", uniqueId))
        {
            UniqueId = uniqueId;
        }
    }
}