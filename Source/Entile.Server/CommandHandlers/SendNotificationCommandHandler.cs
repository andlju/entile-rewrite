using Entile.Server.Commands;
using Entile.Server.Domain;

namespace Entile.Server.CommandHandlers
{
    public class SendNotificationCommandHandler : IMessageHandler<SendNotificationCommand>
    {
        private readonly IRepository<Client> _clientRepository;
        private readonly INotificationSender _notificationSender;
        private readonly ICommandScheduler _commandScheduler;

        public SendNotificationCommandHandler(IRepository<Client> clientRepository, INotificationSender notificationSender, ICommandScheduler commandScheduler)
        {
            _clientRepository = clientRepository;
            _notificationSender = notificationSender;
            _commandScheduler = commandScheduler;
        }

        public void Handle(SendNotificationCommand command)
        {
            var client = _clientRepository.GetById(command.UniqueId);

            if (client == null)
                throw new ClientNotRegisteredException(command.UniqueId);

            client.SendNotification(command.Title, command.Body, _notificationSender, _commandScheduler);

            _clientRepository.SaveChanges(client);
        }
    }
}