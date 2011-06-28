using Entile.Server.Domain;

namespace Entile.Server
{
    public interface IRepository<TDomain> where TDomain : Aggregate<TDomain>, new()
    {
        TDomain GetById(string uniqueId);

        void SaveChanges(TDomain aggregate);
    }
}