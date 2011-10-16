using Entile.Server.Commands;
using Entile.Server.Domain;

namespace Entile.Server.CommandHandlers
{
    public class UnregisterSubscriptionCommandHandler : IMessageHandler<UnregisterSubscriptionCommand>
    {
        private readonly IRepository<Client> _clientRepository;

        public UnregisterSubscriptionCommandHandler(IRepository<Client> clientRepository)
        {
            _clientRepository = clientRepository;
        }

        public void Handle(UnregisterSubscriptionCommand command)
        {
            var client = _clientRepository.GetById(command.ClientId);

            client.UnregisterSubscription(command.SubscriptionId);

            _clientRepository.SaveChanges(client);
        }
    }
}