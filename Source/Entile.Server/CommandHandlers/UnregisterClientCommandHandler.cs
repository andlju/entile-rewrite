using Entile.Server.Commands;
using Entile.Server.Domain;

namespace Entile.Server.CommandHandlers
{
    public class UnregisterClientCommandHandler : IMessageHandler<UnregisterClientCommand>
    {
        private readonly IRepository<Client> _clientRepository;

        public UnregisterClientCommandHandler(IRepository<Client> clientRepository)
        {
            _clientRepository = clientRepository;
        }

        public void Handle(UnregisterClientCommand command)
        {
            var client = _clientRepository.GetById(command.UniqueId);

            if (client != null)
            {
                client.Unregister();
            }
            _clientRepository.SaveChanges(client);
        }

    }
}