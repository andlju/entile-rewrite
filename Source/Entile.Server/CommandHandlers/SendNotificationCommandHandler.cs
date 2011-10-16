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

            client.SendNotification(new ToastNotification()
                                        {
                                            Title = command.Title, 
                                            Body = command.Body, 
                                            NotificationId = command.NotificationId, 
                                            ParamUri = command.ParamUri
                                        }, command.NumberOfAttempts, _notificationSender, _commandScheduler);

            _clientRepository.SaveChanges(client);
        }

        public void Handle(SendRawNotificationCommand command)
        {
            var client = _clientRepository.GetById(command.ClientId);

            if (client == null)
                throw new ClientNotRegisteredException(command.ClientId);

            client.SendNotification(new RawNotification() 
            {
                NotificationId = command.NotificationId,
                RawContent = command.Content
            }, command.NumberOfAttempts, _notificationSender, _commandScheduler);

            _clientRepository.SaveChanges(client);
        }

        public void Handle(SendTileNotificationCommand command)
        {
            var client = _clientRepository.GetById(command.ClientId);

            if (client == null)
                throw new ClientNotRegisteredException(command.ClientId);

            client.SendNotification(new TileNotification() 
            {
                NotificationId = command.NotificationId,
                Title = command.Title,
                Counter = command.Counter,
                BackgroundUri = command.BackgroundUri,
                BackTitle = command.BackTitle,
                BackContent = command.BackContent,
                BackBackgroundUri = command.BackBackgroundUri,
                ParamUri = command.ParamUri
            }, command.NumberOfAttempts, _notificationSender, _commandScheduler);

            _clientRepository.SaveChanges(client);
        }
    }
}