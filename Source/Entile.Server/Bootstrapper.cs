using System;
using System.Threading;
using System.Threading.Tasks;
using Entile.Server.CommandHandlers;
using Entile.Server.Commands;
using Entile.Server.Domain;
using Entile.Server.Events;
using Entile.Server.ViewHandlers;

namespace Entile.Server
{
    public class EntileServer
    {
        private readonly object _lock = new object();
        private bool _isInitialized = false;

        private IBus _commandBus;
        private IMessageRouter _commandRouter;

        private IBus _viewCreatorsBus;
        private IMessageRouter _viewCreatorsEventRouter;

        private IRegistrator _registrator;
        private INotifier _notifier;
        private Task _commandSchedulerTask;
        private Task _createViewsTask;

        public void Initialize()
        {
            var jsonEventSerializer = new JsonMessageSerializer();

            // Register known events to serialize
            jsonEventSerializer.RegisterKnownMessageType<ClientRegisteredEvent>();
            jsonEventSerializer.RegisterKnownMessageType<ClientRegistrationUpdatedEvent>();
            jsonEventSerializer.RegisterKnownMessageType<ClientUnregisteredEvent>();
            jsonEventSerializer.RegisterKnownMessageType<ExtendedInformationItemSetEvent>();
            jsonEventSerializer.RegisterKnownMessageType<ExtendedInformationItemRemovedEvent>();
            jsonEventSerializer.RegisterKnownMessageType<AllExtendedInformationItemsRemovedEvent>();

            jsonEventSerializer.RegisterKnownMessageType<ToastNotificationSucceededEvent>();
            jsonEventSerializer.RegisterKnownMessageType<RawNotificationSucceededEvent>();
            jsonEventSerializer.RegisterKnownMessageType<TileNotificationSucceededEvent>();
            jsonEventSerializer.RegisterKnownMessageType<ToastNotificationFailedEvent>();
            jsonEventSerializer.RegisterKnownMessageType<RawNotificationFailedEvent>();
            jsonEventSerializer.RegisterKnownMessageType<TileNotificationFailedEvent>();

            var jsonCommandSerializer = new JsonMessageSerializer();
            jsonCommandSerializer.RegisterKnownMessageType<SendToastNotificationCommand>();
            jsonCommandSerializer.RegisterKnownMessageType<SendRawNotificationCommand>();
            jsonCommandSerializer.RegisterKnownMessageType<SendTileNotificationCommand>();

            var entityFrameworkEventStore = new EntityFrameworkEventStore(jsonEventSerializer);

            InitializeClientCommands(entityFrameworkEventStore);
            InitializeViewCreators(entityFrameworkEventStore);

            _registrator = new Registrator(_commandBus);
            _notifier = new Notifier(_commandBus);

        }

        private void InitializeClientCommands(EntityFrameworkEventStore entityFrameworkEventStore)
        {
            _commandRouter = new MessageRouter();
            _commandBus = new InProcessBus(_commandRouter);

            var clientRepository = new EventStoreRepository<Client>(entityFrameworkEventStore, _commandBus);

            var notificationSender = new DummyNotificationSender();
            var commandScheduler = new MessageScheduler(entityFrameworkEventStore);

            // Register Command Handlers
            _commandRouter.RegisterHandlersIn(new RegisterClientCommandHandler(clientRepository));
            _commandRouter.RegisterHandlersIn(new UnregisterClientCommandHandler(clientRepository));
            _commandRouter.RegisterHandlersIn(new SetExtendedInformationItemCommandHandler(clientRepository));
            _commandRouter.RegisterHandlersIn(new RemoveExtendedInformationItemsCommandHandler(clientRepository));
            _commandRouter.RegisterHandlersIn(new RemoveAllExtendedInformationItemsCommandHandler(clientRepository));

            _commandRouter.RegisterHandlersIn(new SendNotificationCommandHandler(clientRepository, notificationSender, commandScheduler));


            _commandSchedulerTask = new Task(() =>
                                                 {
                                                     while (true)
                                                     {
                                                         var messages = commandScheduler.GetMessagesToProcess();
                                                         foreach (var message in messages)
                                                         {
                                                             _commandBus.Publish(message);
                                                         }
                                                         Thread.Sleep(5000);
                                                     }
                                                 });
            _commandSchedulerTask.Start();
        }

        private void InitializeViewCreators(EntityFrameworkEventStore entityFrameworkEventStore)
        {
            _viewCreatorsEventRouter = new MessageRouter();
            _viewCreatorsBus = new InProcessBus(_viewCreatorsEventRouter);

            // Register Event Handlers
            _viewCreatorsEventRouter.RegisterHandlersIn(new ClientViewHandler());
            _viewCreatorsEventRouter.RegisterHandlersIn(new ExtendedInformationViewHandler());

            var dispatcher = new EventStreamDispatcher(_viewCreatorsBus, entityFrameworkEventStore);

            _createViewsTask = new Task(() =>
                                            {
                                                var lastUpdate = entityFrameworkEventStore.GetLastUpdate("Views");
                                                while (true)
                                                {
                                                    lastUpdate = dispatcher.Dispatch(lastUpdate);
                                                    entityFrameworkEventStore.SetLastUpdate("Views", lastUpdate);
                                                    Thread.Sleep(1000);
                                                }
                                            });
            _createViewsTask.Start();
        }

        public IRegistrator Registrator
        {
            get
            {
                EnsureInitialized();
                return _registrator;
            }
        }

        public INotifier Notifier
        {
            get
            {
                EnsureInitialized();
                return _notifier;
            }
        }

        private void EnsureInitialized()
        {
            if (_isInitialized)
                return;

            lock (_lock)
            {
                if (!_isInitialized)
                {
                    Initialize();
                    _isInitialized = true;
                }
            }
        }
    }

    public class Bootstrapper
    {
        private static EntileServer _currentServer;

        public static EntileServer CurrentServer
        {
            get
            {
                return _currentServer ?? (_currentServer = new EntileServer());
            }

        }
    }
}