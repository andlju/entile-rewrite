using Entile.Server.Commands;
using Entile.Server.Domain;

namespace Entile.Server.CommandHandlers
{
    public class RemoveAllExtendedInformationItemsCommandHandler : IMessageHandler<RemoveAllExtendedInformationItemsCommand>
    {
        private readonly IRepository<Client> _clientRepository;

        public RemoveAllExtendedInformationItemsCommandHandler(IRepository<Client> clientRepository)
        {
            _clientRepository = clientRepository;
        }

        public void Handle(RemoveAllExtendedInformationItemsCommand command)
        {
            var client = _clientRepository.GetById(command.UniqueId);

            client.RemoveAllExtendedInformationItems();

            _clientRepository.SaveChanges(client);
        }
    }
}