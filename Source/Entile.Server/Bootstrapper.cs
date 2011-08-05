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

            var jsonCommandSerializer = new JsonMessageSerializer();
            jsonCommandSerializer.RegisterKnownMessageType<SendNotificationCommand>();

            var entityFrameworkEventStore = new EntityFrameworkEventStore(jsonEventSerializer);

            InitializeRegistrator(entityFrameworkEventStore);
            InitializeViewCreators(entityFrameworkEventStore);
        }

        private void InitializeRegistrator(EntityFrameworkEventStore entityFrameworkEventStore)
        {
            _commandRouter = new MessageRouter();
            _commandBus = new InProcessBus(_commandRouter);

            var clientRepository = new EventStoreRepository<Client>(entityFrameworkEventStore, _commandBus);

            // Register Command Handlers
            _commandRouter.RegisterHandlersIn(new RegisterClientCommandHandler(clientRepository));
            _commandRouter.RegisterHandlersIn(new UnregisterClientCommandHandler(clientRepository));
            _commandRouter.RegisterHandlersIn(new SetExtendedInformationItemCommandHandler(clientRepository));
            _commandRouter.RegisterHandlersIn(new RemoveExtendedInformationItemsCommandHandler(clientRepository));
            _commandRouter.RegisterHandlersIn(new RemoveAllExtendedInformationItemsCommandHandler(clientRepository));
            

            _registrator = new Registrator(_commandBus);
        }

        private void InitializeViewCreators(EntityFrameworkEventStore entityFrameworkEventStore)
        {
            _viewCreatorsEventRouter = new MessageRouter();
            _viewCreatorsBus = new InProcessBus(_viewCreatorsEventRouter);

            // Register Event Handlers
            _viewCreatorsEventRouter.RegisterHandlersIn(new ClientViewHandler());
            _viewCreatorsEventRouter.RegisterHandlersIn(new ExtendedInformationViewHandler());

            var dispatcher = new EventStreamDispatcher(_viewCreatorsBus, entityFrameworkEventStore);

            var createViewsTask = new Task(() =>
                                               {
                                                   var lastUpdate = entityFrameworkEventStore.GetLastUpdate("Views");
                                                   while (true)
                                                   {
                                                       lastUpdate = dispatcher.Dispatch(lastUpdate);
                                                       entityFrameworkEventStore.SetLastUpdate("Views", lastUpdate);
                                                       Thread.Sleep(1000);
                                                   }
                                               });
            createViewsTask.Start();
        }

        public IRegistrator Registrator
        {
            get
            {
                EnsureInitialized();
                return _registrator;
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