using Entile.Server.Commands;
using Entile.Server.Domain;

namespace Entile.Server.CommandHandlers
{
    public class SetExtendedInformationCommandHandler : IMessageHandler<SetExtendedInformationCommand>
    {
        private readonly IRepository<Client> _clientRepository;

        public SetExtendedInformationCommandHandler(IRepository<Client> clientRepository)
        {
            _clientRepository = clientRepository;
        }

        public void Handle(SetExtendedInformationCommand command)
        {
            var client = _clientRepository.GetById(command.UniqueId);

            client.SetExtendedInformationItem(command.Key, command.Value);

            _clientRepository.SaveChanges(client);
        }
    }
}