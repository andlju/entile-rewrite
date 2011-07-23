namespace Entile.Server.Commands
{
    public class SetExtendedInformationCommand : CommandBase
    {
        public string UniqueId { get; private set; }
        public string Key { get; private set; }
        public string Value { get; private set; }

        public SetExtendedInformationCommand(string uniqueId, string key, string value)
        {
            UniqueId = uniqueId;
            Key = key;
            Value = value;
        }
    }
}