using Entile.Server.Commands;
using Entile.Server.Domain;

namespace Entile.Server.CommandHandlers
{
    public class SetExtendedInformationItemCommandHandler : IMessageHandler<SetExtendedInformationItemCommand>
    {
        private readonly IRepository<Client> _clientRepository;

        public SetExtendedInformationItemCommandHandler(IRepository<Client> clientRepository)
        {
            _clientRepository = clientRepository;
        }

        public void Handle(SetExtendedInformationItemCommand command)
        {
            var client = _clientRepository.GetById(command.ClientId);

            client.SetExtendedInformationItem(command.Key, command.Value);

            _clientRepository.SaveChanges(client);
        }
    }
}