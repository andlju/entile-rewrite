using Entile.Server.Commands;
using Entile.Server.Domain;

namespace Entile.Server.CommandHandlers
{
    public class RemoveAllExtendedInformationCommandHandler : IMessageHandler<RemoveAllExtendedInformationCommand>
    {
        private readonly IRepository<Client> _clientRepository;

        public RemoveAllExtendedInformationCommandHandler(IRepository<Client> clientRepository)
        {
            _clientRepository = clientRepository;
        }

        public void Handle(RemoveAllExtendedInformationCommand command)
        {
            var client = _clientRepository.GetById(command.UniqueId);

            client.RemoveAllExtendedInformationItems();

            _clientRepository.SaveChanges(client);
        }
    }
}