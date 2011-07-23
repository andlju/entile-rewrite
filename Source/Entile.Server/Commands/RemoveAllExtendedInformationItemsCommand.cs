namespace Entile.Server.Commands
{
    public class RemoveAllExtendedInformationItemsCommand : CommandBase
    {
        public string UniqueId { get; private set; }

        public RemoveAllExtendedInformationItemsCommand(string uniqueId)
        {
            UniqueId = uniqueId;
        }
    }
}