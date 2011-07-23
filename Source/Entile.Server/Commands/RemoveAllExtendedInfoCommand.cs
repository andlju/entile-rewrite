namespace Entile.Server.Commands
{
    public class RemoveAllExtendedInformationCommand : CommandBase
    {
        public string UniqueId { get; private set; }

        public RemoveAllExtendedInformationCommand(string uniqueId)
        {
            UniqueId = uniqueId;
        }
    }
}