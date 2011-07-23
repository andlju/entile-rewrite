namespace Entile.Server.Commands
{
    public class RemoveExtendedInformationCommand : CommandBase
    {
        public string UniqueId { get; private set; }
        public string Key { get; private set; }

        public RemoveExtendedInformationCommand(string uniqueId, string key)
        {
            UniqueId = uniqueId;
            Key = key;
        }
    }
}