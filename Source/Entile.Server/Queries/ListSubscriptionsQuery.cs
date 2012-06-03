using System.Collections.Generic;

namespace Entile.Server.Queries
{
    public class ListSubscriptionsQuery : IMessage
    {
        public Dictionary<string, string> Match { get; set; }
    }
}