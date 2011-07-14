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
        private IBus _bus;
        private IMessageRouter _messageRouter;
        private IRegistrator _registrator;

        public void Initialize()
        {
            _messageRouter = new MessageRouter();
            _bus = new InProcessBus(_messageRouter);

            var jsonEventSerializer = new JsonEventSerializer();
            
            // Register known events to serialize
            jsonEventSerializer.RegisterKnownEventType<ClientRegisteredEvent>();
            jsonEventSerializer.RegisterKnownEventType<ClientRegistrationUpdatedEvent>();
            jsonEventSerializer.RegisterKnownEventType<ClientUnregisteredEvent>();
            jsonEventSerializer.RegisterKnownEventType<ExtendedInformationItemSetEvent>();
            jsonEventSerializer.RegisterKnownEventType<ExtendedInformationItemRemovedEvent>();
            jsonEventSerializer.RegisterKnownEventType<AllExtendedInformationItemsRemovedEvent>();

            var entityFrameworkEventStore = new EntityFrameworkEventStore(jsonEventSerializer);

            var clientRepository = new EventStoreRepository<Client>(entityFrameworkEventStore, _bus);

            // Register Command Handlers
            _messageRouter.RegisterHandlersIn(new RegisterClientCommandHandler(clientRepository));
            _messageRouter.RegisterHandlersIn(new UnregisterClientCommandHandler(clientRepository));

            // Register Event Handlers
            _messageRouter.RegisterHandlersIn(new RegistrationView());

            _registrator = new Registrator(_bus);
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