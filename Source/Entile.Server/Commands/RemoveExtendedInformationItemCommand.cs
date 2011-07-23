namespace Entile.Server.Commands
{
    public class RemoveExtendedInformationItemCommand : CommandBase
    {
        public string UniqueId { get; private set; }
        public string Key { get; private set; }

        public RemoveExtendedInformationItemCommand(string uniqueId, string key)
        {
            UniqueId = uniqueId;
            Key = key;
        }
    }
}