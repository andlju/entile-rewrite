using System;
using CommonDomain;
using CommonDomain.Persistence;

namespace Entile.Server.Domain
{
    public class AggregateFactory :IConstructAggregates 
    {
        public IAggregate Build(Type type, Guid id, IMemento snapshot)
        {
            return Activator.CreateInstance(type) as IAggregate;
        }
    }
}