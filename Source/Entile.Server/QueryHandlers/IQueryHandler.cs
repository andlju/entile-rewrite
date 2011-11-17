namespace Entile.Server.QueryHandlers
{
    public interface IQueryHandler<TQuery>
    {
        dynamic Handle(TQuery query);
    }
}