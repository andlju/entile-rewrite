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
using Entile.Server.ViewHandlers;
using EventStore;
using EventStore.Dispatcher;

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

            var repository = new EventStoreRepository(
                Wireup.Init().
                    UsingSqlPersistence("EntileEventStore").InitializeStorageEngine().
                    UsingJsonSerialization().UsingAsynchronousDispatcher(
                        new DelegateMessagePublisher(c =>
                                                         {
                                                             foreach (var e in c.Events)
                                                             {
                                                                 _viewCreatorsBus.Publish(e.Body as IMessage);
                                                             }
                                                         })).
                    Build(),
                new AggregateFactory(), new ConflictDetector());


            InitializeClientCommands(repository);
            InitializeViewCreators();

            _registrator = new Registrator(_commandBus);
            _notifier = new Notifier(_commandBus);

        }

        private void InitializeClientCommands(IRepository repository)
        {
            _commandRouter = new MessageRouter();
            _commandBus = new InProcessBus(_commandRouter);


            var notificationSender = new DummyNotificationSender();
            var commandScheduler = new MessageScheduler(null);

            // Register Command Handlers
            _commandRouter.RegisterHandlersIn(new RegisterClientCommandHandler(repository));
            _commandRouter.RegisterHandlersIn(new UnregisterClientCommandHandler(repository));
            _commandRouter.RegisterHandlersIn(new SubscribeCommandHandler(repository));
            _commandRouter.RegisterHandlersIn(new UnsubscribeCommandHandler(repository));

            _commandRouter.RegisterHandlersIn(new SendNotificationCommandHandler(repository, notificationSender, commandScheduler));

            /*_commandSchedulerTask = new Task(() =>
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
            _commandSchedulerTask.Start();*/
        }

        private void InitializeViewCreators()
        {
            _viewCreatorsEventRouter = new MessageRouter();
            _viewCreatorsBus = new InProcessBus(_viewCreatorsEventRouter);

            // Register Event Handlers
            _viewCreatorsEventRouter.RegisterHandlersIn(new ClientViewHandler());
            _viewCreatorsEventRouter.RegisterHandlersIn(new SubscriptionViewHandler());
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

        public IBus CommandBus
        {
            get
            {
                EnsureInitialized();
                return _commandBus;
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