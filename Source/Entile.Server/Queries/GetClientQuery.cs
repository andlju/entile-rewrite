using System;
using Entile.Server.ViewHandlers;

namespace Entile.Server.Queries
{
    public class GetClientQuery : IMessage
    {
        public Guid ClientId;
    }
}