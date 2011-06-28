namespace Entile.Server.Commands
{
    public class UnregisterClientCommand : ICommand
    {
        public string UniqueId { get; private set; }

        public UnregisterClientCommand(string uniqueId)
        {
            UniqueId = uniqueId;
        }
    }
}