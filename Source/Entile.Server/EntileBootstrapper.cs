using System;
using System.Collections.Generic;
using System.IO;
using Entile.Server.CommandHandlers;
using Entile.Server.Commands;
using Entile.Server.Domain;
using Entile.Server.Events;

namespace Entile.Server
{
    public static class EntileBootstrapper
    {
        public static Func<IMessageRouter, IBus> BusFactoryMethod = DefaultBusFactory;
        public static Func<IMessageRouter> MessageRouterFactoryMethod = DefaultMessageRouterFactory;
        public static Func<string, IEventStore> EventStoreFactoryMethod = DefaultEventStoreFactory;
        public static Func<IRepository<Client>> RegistrationRepositoryFactoryMethod = DefaultRegistrationRepositoryFactory;
        public static Func<IEventSerializer> EventSerializerFactoryMethod = DefaultEventSerializerFactory;

        public static IRegistrator CreateRegistrator()
        {
            var commandMessageRouter = MessageRouterFactoryMethod();

            var bus = BusFactoryMethod(commandMessageRouter);
            var registrationRepository = RegistrationRepositoryFactoryMethod();

            commandMessageRouter.RegisterHandler<RegisterClientCommand>(new RegisterClientCommandHandler(registrationRepository).Execute);
            commandMessageRouter.RegisterHandler<UnregisterClientCommand>(new UnregisterClientCommandHandler(registrationRepository).Execute);

            var registrator = new Registrator(bus);

            return registrator;
        }

        public static IEventSerializer DefaultEventSerializerFactory()
        {
            var eventSerializer = new JsonEventSerializer();

            eventSerializer.RegisterKnownEventType<ClientRegisteredEvent>();
            eventSerializer.RegisterKnownEventType<ClientUnregisteredEvent>();
            eventSerializer.RegisterKnownEventType<ClientRegistrationUpdatedEvent>();
            eventSerializer.RegisterKnownEventType<ExtendedInformationItemSetEvent>();
            eventSerializer.RegisterKnownEventType<ExtendedInformationItemRemovedEvent>();
            eventSerializer.RegisterKnownEventType<AllExtendedInformationItemsRemovedEvent>();
            
            return eventSerializer;
        }

        public static IEventStore DefaultEventStoreFactory(string name)
        {
            var serializer = DefaultEventSerializerFactory();
            return new EntityFrameworkEventStore(serializer);
        }

        public static IRepository<Client> DefaultRegistrationRepositoryFactory()
        {
            var eventStore = EventStoreFactoryMethod("Registration");
            return new EventStoreRepository<Client>(eventStore);
        }

        public static IMessageRouter DefaultMessageRouterFactory()
        {
            return new MessageRouter();
        }

        public static IBus DefaultBusFactory(IMessageRouter messageRouter)
        {
            return new InProcessBus(messageRouter);
        }
    }
}