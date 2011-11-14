using System.Collections.Generic;
using CommonDomain.Persistence;
using Entile.Server.CommandHandlers;
using Entile.Server.Commands;
using Entile.Server.Domain;
using Entile.Server.Events;
using Microsoft.VisualStudio.TestTools.UnitTesting;



namespace Entile.Server.Tests.Domain.ClientTests
{
    [TestClass]
    public class When_Registering_Already_Registered_Client : With<Client, RegisterClientCommand>
    {
        protected override IMessageHandler<RegisterClientCommand> CreateHandler(IRepository repository)
        {
            return new RegisterClientCommandHandler(repository);
        }

        protected override IEnumerable<IEvent> Given()
        {
            yield return new ClientRegisteredEvent(UniqueId, "http://my.channel.com");
        }

        protected override RegisterClientCommand When()
        {
            return new RegisterClientCommand(UniqueId, "http://new.channel.com");
        }

        [TestMethod]
        public void Then_ClientRegistrationUpdatedEvent_Is_Sent()
        {
            AssertEvent.IsType<ClientRegistrationUpdatedEvent>(0);
        }

        [TestMethod]
        public void Then_The_UniqueId_On_The_Event_Is_Correct()
        {
            AssertEvent.Contents<ClientRegistrationUpdatedEvent>(0,
                                                                 ev => Assert.AreEqual(UniqueId, ev.AggregateId));
        }

        [TestMethod]
        public void Then_The_NotificationChannel_On_The_Event_Is_Correct()
        {
            AssertEvent.Contents<ClientRegistrationUpdatedEvent>(0,
                                                                 ev => Assert.AreEqual("http://new.channel.com", ev.NotificationChannel));
        }
    }
}