using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CommonDomain.Core;
using CommonDomain.Persistence;
using CommonDomain.Persistence.EventStore;
using Entile.Server.CommandHandlers;
using Entile.Server.Commands;
using Entile.Server.Domain;
using Entile.Server.Events;
using Entile.Server.QueryHandlers;
using Entile.Server.ViewHandlers;
using EventStore;
using EventStore.Dispatcher;

namespace Entile.Server
{
    public class EntileServer
    {
        private readonly object _lock = new object();
        private bool _isInitialized = false;

        private IMessageDispatcher _commandDispatcher;
        private IRouter<Action<IMessage>> _commandRouter;

        private IMessageDispatcher _viewCreatorsDispatcher;
        private IRouter<Action<IMessage>> _viewCreatorsEventRouter;

        public void Initialize()
        {
            InitializeViewCreators();

            var repository = new EventStoreRepository(
                Wireup.Init().
                    UsingSqlPersistence("EntileEventStore").InitializeStorageEngine().
                    UsingJsonSerialization().UsingAsynchronousDispatchScheduler(
                        new DelegateMessageDispatcher(c =>
                                                         {
                                                             foreach (var e in c.Events)
                                                             {
                                                                 _viewCreatorsDispatcher.Dispatch(e.Body as IMessage);
                                                             }
                                                         })).
                    Build(),
                new AggregateFactory(), new ConflictDetector());


            InitializeClientCommands(repository);
        }

        private void InitializeClientCommands(IRepository repository)
        {
            _commandRouter = new MessageRouter<Action<IMessage>>();
            _commandDispatcher = new InProcessMessageDispatcher(_commandRouter);


            var notificationSender = new DummyNotificationSender();
            var commandScheduler = new MessageScheduler(null);

            // Register Command Handlers
            _commandRouter.RegisterHandlersIn(new RegisterClientCommandHandler(repository));
            _commandRouter.RegisterHandlersIn(new UnregisterClientCommandHandler(repository));
            _commandRouter.RegisterHandlersIn(new SubscribeCommandHandler(repository));
            _commandRouter.RegisterHandlersIn(new UnsubscribeCommandHandler(repository));

            _commandRouter.RegisterHandlersIn(new SendNotificationCommandHandler(repository, notificationSender, commandScheduler));

        }

        private void InitializeViewCreators()
        {
            _viewCreatorsEventRouter = new MessageRouter<Action<IMessage>>();
            _viewCreatorsDispatcher = new InProcessMessageDispatcher(_viewCreatorsEventRouter);

            // Register Event Handlers
            _viewCreatorsEventRouter.RegisterHandlersIn(new ClientViewHandler());
            _viewCreatorsEventRouter.RegisterHandlersIn(new SubscriptionViewHandler());
        }

        public IMessageDispatcher CommandDispatcher
        {
            get
            {
                EnsureInitialized();
                return _commandDispatcher;
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