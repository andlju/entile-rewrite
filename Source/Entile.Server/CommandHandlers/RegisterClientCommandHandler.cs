using Entile.Server.Commands;
using Entile.Server.Domain;

namespace Entile.Server.CommandHandlers
{
    public class RegisterClientCommandHandler : IMessageHandler<RegisterClientCommand>
    {
        private readonly IRepository<Client> _clientRepository;

        public RegisterClientCommandHandler(IRepository<Client> clientRepository)
        {
            _clientRepository = clientRepository;
        }

        public void Handle(RegisterClientCommand command)
        {
            var registration = _clientRepository.GetById(command.UniqueId);

            if (registration != null)
            {
                registration.UpdateRegistration(command.NotificationChannel);
            } 
            else
            {
                registration = new Client(command.UniqueId, command.NotificationChannel);
            }

            _clientRepository.SaveChanges(registration);
        }
    }
}