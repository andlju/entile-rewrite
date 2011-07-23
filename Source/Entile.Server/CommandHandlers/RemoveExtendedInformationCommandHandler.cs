using Entile.Server.Commands;
using Entile.Server.Domain;

namespace Entile.Server.CommandHandlers
{
    public class RemoveExtendedInformationCommandHandler : IMessageHandler<RemoveExtendedInformationCommand>
    {
        private readonly IRepository<Client> _clientRepository;

        public RemoveExtendedInformationCommandHandler(IRepository<Client> clientRepository)
        {
            _clientRepository = clientRepository;
        }

        public void Handle(RemoveExtendedInformationCommand command)
        {
            var client = _clientRepository.GetById(command.UniqueId);

            client.RemoveExtendedInformationItem(command.Key);
            
            _clientRepository.SaveChanges(client);
        }
    }
}