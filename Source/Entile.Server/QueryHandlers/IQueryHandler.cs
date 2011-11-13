namespace Entile.Server.QueryHandlers
{
    public interface IQueryHandler<TQuery>
    {
        object Handle(TQuery query);
    }
}