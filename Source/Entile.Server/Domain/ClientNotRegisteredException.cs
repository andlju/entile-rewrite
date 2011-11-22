using System;
using System.Globalization;

namespace Entile.Server.Domain
{
    public class ClientNotRegisteredException : Exception
    {
        public Guid ClientId { get; private set; }

        public ClientNotRegisteredException(Guid clientId) :
            base(string.Format(CultureInfo.InvariantCulture, "Client with Id {0} was not found", clientId))
        {
            ClientId = clientId;
        }
    }
}