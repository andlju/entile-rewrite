using Entile.Server.Commands;
using Entile.Server.Domain;

namespace Entile.Server.CommandHandlers
{
    public class RemoveExtendedInformationItemsCommandHandler : IMessageHandler<RemoveExtendedInformationItemCommand>
    {
        private readonly IRepository<Client> _clientRepository;

        public RemoveExtendedInformationItemsCommandHandler(IRepository<Client> clientRepository)
        {
            _clientRepository = clientRepository;
        }

        public void Handle(RemoveExtendedInformationItemCommand command)
        {
            var client = _clientRepository.GetById(command.UniqueId);

            client.RemoveExtendedInformationItem(command.Key);
            
            _clientRepository.SaveChanges(client);
        }
    }
}