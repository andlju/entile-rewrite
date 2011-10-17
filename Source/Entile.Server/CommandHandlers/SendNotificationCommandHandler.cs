using Entile.Server.Commands;
using Entile.Server.Domain;

namespace Entile.Server.CommandHandlers
{
    public class SendNotificationCommandHandler :
        IMessageHandler<SendToastNotificationCommand>,
        IMessageHandler<SendRawNotificationCommand>,
        IMessageHandler<SendTileNotificationCommand>
    {
        private readonly IRepository<Client> _clientRepository;
        private readonly INotificationSender _notificationSender;
        private readonly IMessageScheduler _commandScheduler;

        public SendNotificationCommandHandler(IRepository<Client> clientRepository, INotificationSender notificationSender, IMessageScheduler commandScheduler)
        {
            _clientRepository = clientRepository;
            _notificationSender = notificationSender;
            _commandScheduler = commandScheduler;
        }

        public void Handle(SendToastNotificationCommand command)
        {
            var client = _clientRepository.GetById(command.ClientId);

            if (client == null)
                throw new ClientNotRegisteredException(command.ClientId);

            client.SendToastNotification(
                command.SubscriptionId, 
                command.NotificationId, 
                command.Title, 
                command.Body, 
                command.NumberOfAttempts, 
                _notificationSender, _commandScheduler);

            _clientRepository.SaveChanges(client);
        }

        public void Handle(SendRawNotificationCommand command)
        {
            var client = _clientRepository.GetById(command.ClientId);

            if (client == null)
                throw new ClientNotRegisteredException(command.ClientId);

            client.SendRawNotification(
                command.SubscriptionId, 
                command.NotificationId,
                command.Content,
                command.NumberOfAttempts, 
                _notificationSender, _commandScheduler);

            _clientRepository.SaveChanges(client);
        }

        public void Handle(SendTileNotificationCommand command)
        {
            var client = _clientRepository.GetById(command.ClientId);

            if (client == null)
                throw new ClientNotRegisteredException(command.ClientId);

            client.SendTileNotification(
                command.SubscriptionId,
                command.NotificationId,
                command.Title,
                command.Counter,
                command.BackgroundUri,
                command.BackTitle,
                command.BackContent,
                command.BackBackgroundUri,
                command.NumberOfAttempts,
                _notificationSender, _commandScheduler);

            _clientRepository.SaveChanges(client);
        }
    }
}