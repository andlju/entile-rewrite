namespace Entile.Server.Events
{
    public interface IEvent
    {
        string UniqueId { get; set; }
        int SequenceNumber { get; set; }
    }
}