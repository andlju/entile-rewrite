using System;
using System.Collections.Generic;

namespace Entile.Server.Queries
{
    
    public class GetClientQuery : IMessage
    {
        public Guid ClientId;
    }
}