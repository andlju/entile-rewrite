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
            var registration = _clientRepository.GetById(command.UniqueId);

            if (registration != null)
            {
                registration.Unregister();
            }
            _clientRepository.SaveChanges(registration);
        }

    }
}