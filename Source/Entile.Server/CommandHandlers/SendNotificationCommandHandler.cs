using Entile.Server.Commands;
using Entile.Server.Domain;

namespace Entile.Server.CommandHandlers
{
    public class SendNotificationCommandHandler : IMessageHandler<SendToastNotificationCommand>
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
            var client = _clientRepository.GetById(command.UniqueId);

            if (client == null)
                throw new ClientNotRegisteredException(command.UniqueId);

            client.SendNotification(new ToastNotification()
                                        {
                                            Title = command.Title, 
                                            Body = command.Body, 
                                            UniqueNotificationId = command.NotificationId, 
                                            ParamUri = command.ParamUri
                                        }, _notificationSender, _commandScheduler);

            _clientRepository.SaveChanges(client);
        }
    }
}