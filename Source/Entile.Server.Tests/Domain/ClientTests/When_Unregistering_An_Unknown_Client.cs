using System;
using System.Collections.Generic;
using CommonDomain.Persistence;
using Entile.Server.CommandHandlers;
using Entile.Server.Commands;
using Entile.Server.Domain;
using Entile.Server.Events;
using Xunit;

namespace Entile.Server.Tests.Domain.ClientTests
{
    public class When_Unregistering_An_Unknown_Client : With<Client, UnregisterClientCommand>
    {
        protected override IMessageHandler<UnregisterClientCommand> CreateHandler(IRepository repository)
        {
            return new UnregisterClientCommandHandler(repository);
        }

        protected override UnregisterClientCommand When()
        {
            return new UnregisterClientCommand(UniqueId);
        }

        [Fact]
        public void Then_No_Event_Is_Sent()
        {
            Assert.Null(Events);
        }

        [Fact]
        public void Then_ClientNotRegisteredException_Should_Be_Thrown()
        {
            Assert.IsType<InvalidOperationException>(ExceptionThrown);
        }

        [Fact]
        public void Then_UniqueId_In_The_Exception_Is_Correct()
        {
            Assert.True(((InvalidOperationException)ExceptionThrown).Message.Contains(UniqueId.ToString()));
        }
    }
}