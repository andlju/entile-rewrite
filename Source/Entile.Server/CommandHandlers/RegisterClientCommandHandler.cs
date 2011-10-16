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
            var client = _clientRepository.GetById(command.ClientId);

            if (client != null)
            {
                client.UpdateRegistration(command.NotificationChannel);
            } 
            else
            {
                client = new Client(command.ClientId, command.NotificationChannel); 
            }

            _clientRepository.SaveChanges(client);
        }
    }
}