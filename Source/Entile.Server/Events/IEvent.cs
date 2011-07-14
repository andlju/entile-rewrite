namespace Entile.Server.Events
{
    public interface IEvent : IMessage
    {
        string UniqueId { get; set; }
        int SequenceNumber { get; set; }
    }
}