using Entile.Server.Commands;
using Entile.Server.Domain;

namespace Entile.Server.CommandHandlers
{
    public class RegisterSubscriptionCommandHandler : IMessageHandler<RegisterSubscriptionCommand>
    {
        private IRepository<Client> _clientRepository;

        public RegisterSubscriptionCommandHandler(IRepository<Client> clientRepository)
        {
            _clientRepository = clientRepository;
        }

        public void Handle(RegisterSubscriptionCommand command)
        {
            var client = _clientRepository.GetById(command.ClientId);

            client.RegisterSubscription(command.SubscriptionId, command.Kind, command.Uri,
                                           command.ExtendedInformation);
            
            _clientRepository.SaveChanges(client);
        }
    }
}