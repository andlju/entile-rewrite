using System;
using System.Collections.Generic;
using System.Linq;
using Entile.Server.Domain;
using Entile.Server.Events;
using Xunit;

namespace Entile.Server.Tests.Domain
{
    // ReSharper disable InconsistentNaming

    public class When_Creating_New_Client : With<Client>
    {
        protected override Client Create()
        {
            return new Client("1234", "http://my.channel.com");
        }

        [Fact]
        public void Then_ClientRegisteredEvent_Is_Sent()
        { 
            AssertEvent.IsType<ClientRegisteredEvent>(0);
        }

        [Fact]
        public void Then_The_UniqueId_On_The_Event_Is_Correct()
        {
            AssertEvent.Contents<ClientRegisteredEvent>(0, 
                ev => Assert.Equal("1234", ev.UniqueId)
                );
        }

        [Fact]
        public void Then_The_NotificationChannel_On_The_Event_Is_Correct()
        {
            AssertEvent.Contents<ClientRegisteredEvent>(0, 
                ev => Assert.Equal("http://my.channel.com", ev.NotificationChannel)
                );
        }
    }

    public class When_Updating_Registration_On_Registered_Client : With<Client>
    {
        protected override IEnumerable<IEvent> Given()
        {
            yield return new ClientRegisteredEvent("1234", "http://my.channel.com");
        }

        protected override void When(Client client)
        {
            client.UpdateRegistration("http://new.channel.com");
        }

        [Fact]
        public void Then_ClientRegistrationUpdatedEvent_Is_Sent()
        {
            AssertEvent.IsType<ClientRegistrationUpdatedEvent>(0);
        }

        [Fact]
        public void Then_The_UniqueId_On_The_Event_Is_Correct()
        {
            AssertEvent.Contents<ClientRegistrationUpdatedEvent>(0,
               ev => Assert.Equal("1234", ev.UniqueId));
        }

        [Fact]
        public void Then_The_NotificationChannel_On_The_Event_Is_Correct()
        {
            AssertEvent.Contents<ClientRegistrationUpdatedEvent>(0,
               ev => Assert.Equal("http://new.channel.com", ev.NotificationChannel));
        }
    }

    public class When_Unregistering_A_Registered_Client : With<Client>
    {
        protected override IEnumerable<IEvent> Given()
        {
            yield return new ClientRegisteredEvent("1234", "http://my.channel.com");
        }

        protected override void When(Client client)
        {
            client.Unregister();
        }

        [Fact]
        public void Then_ClientUnregisteredEvent_Is_Sent()
        {
            AssertEvent.IsType<ClientUnregisteredEvent>(0);
        }

        [Fact]
        public void Then_The_UniqueId_On_The_Event_Is_Correct()
        {
            AssertEvent.Contents<ClientUnregisteredEvent>(0,
               ev => Assert.Equal("1234", ev.UniqueId));
        }
    }

    public class When_Unregistering_An_Unregistered_Client : With<Client>
    {
        protected override IEnumerable<IEvent> Given()
        {
            yield return new ClientRegisteredEvent("1234", "http://my.channel.com");
            yield return new ClientUnregisteredEvent();
        }

        protected override void When(Client client)
        {
            client.Unregister();
        }

        [Fact]
        public void Then_ClientNotRegisteredException_Should_Be_Thrown()
        {
            Assert.IsType<ClientNotRegisteredException>(ExceptionThrown);
        }

        [Fact]
        public void Then_UniqueId_In_The_Exception_Is_Correct()
        {
            Assert.Equal("1234", ((ClientNotRegisteredException)ExceptionThrown).UniqueId);
        }
    }

    public class When_Setting_Extended_Info_On_Registered_Client : With<Client>
    {
        protected override IEnumerable<IEvent> Given()
        {
            yield return new ClientRegisteredEvent("1234", "http://my.channel.com");
        }

        protected override void When(Client target)
        {
            target.SetExtendedInformationItem("MyKey", "MyValue");
        }

        [Fact]
        public void Then_ExtendedInformationSetEvent_Is_Sent()
        {
            AssertEvent.IsType<ExtendedInformationItemSetEvent>(0);
        }

        [Fact]
        public void Then_UniqueId_On_The_Event_Is_Correct()
        {
            AssertEvent.Contents<ExtendedInformationItemSetEvent>(0,
                ev => Assert.Equal("1234", ev.UniqueId));
        }

        [Fact]
        public void Then_Key_On_The_Event_Is_Correct()
        {
            AssertEvent.Contents<ExtendedInformationItemSetEvent>(0,
                ev => Assert.Equal("MyKey", ev.Key));
        }

        [Fact]
        public void Then_Value_On_The_Event_Is_Correct()
        {
            AssertEvent.Contents<ExtendedInformationItemSetEvent>(0,
                ev => Assert.Equal("MyValue", ev.Value));
        }
    }

    public class When_Setting_Extended_Information_On_Unregistered_Client: With<Client>
    {
        protected override IEnumerable<IEvent> Given()
        {
            yield return new ClientRegisteredEvent("1234", "http://my.channel.com");
            yield return new ClientUnregisteredEvent();
        }

        protected override void When(Client target)
        {
            target.SetExtendedInformationItem("MyKey", "MyValue");
        }

        [Fact]
        public void Then_ClientNotRegisteredException_Should_Be_Thrown()
        {
            Assert.IsType<ClientNotRegisteredException>(ExceptionThrown);
        }

        [Fact]
        public void Then_UniqueId_In_The_Exception_Is_Correct()
        {
            Assert.Equal("1234", ((ClientNotRegisteredException)ExceptionThrown).UniqueId);
        }
    }

    public class When_Removing_Extended_Info_On_Registered_Client_With_Extended_Info_Item : With<Client>
    {
        protected override IEnumerable<IEvent> Given()
        {
            yield return new ClientRegisteredEvent("1234", "http://my.channel.com");
            yield return new ExtendedInformationItemSetEvent("MyKey", "MyValue");
        }

        protected override void When(Client target)
        {
            target.RemoveExtendedInformationItem("MyKey");
        }

        [Fact]
        public void Then_ExtendedInformationItemRemovedEvent_Is_Sent()
        {
            AssertEvent.IsType<ExtendedInformationItemRemovedEvent>(0);
        }

        [Fact]
        public void Then_UniqueId_On_Event_Is_Correct()
        {
            AssertEvent.Contents<ExtendedInformationItemRemovedEvent>(0,
                ev => Assert.Equal("1234", ev.UniqueId));
        }

        [Fact]
        public void Then_Key_On_Event_Is_Correct()
        {
            AssertEvent.Contents<ExtendedInformationItemRemovedEvent>(0, 
                ev => Assert.Equal("MyKey", ev.Key));
        }
    }

    public class When_Removing_ExtendedInformationItem_On_Registered_Client_With_No_Such_Item : With<Client>
    {
        protected override IEnumerable<IEvent> Given()
        {
            yield return new ClientRegisteredEvent("1234", "http://my.channel.com");
        }

        protected override void When(Client target)
        {
            target.RemoveExtendedInformationItem("MyKey");
        }

        [Fact]
        public void Then_InvalidExtendedInformationItemException_Is_Thrown()
        {
            Assert.IsType<InvalidExtendedInformationItemException>(ExceptionThrown);
        }

        [Fact]
        public void Then_UniqueId_In_The_Exception_Is_Correct()
        {
            Assert.Equal("1234", ((InvalidExtendedInformationItemException)ExceptionThrown).UniqueId);
        }

        [Fact]
        public void Then_Key_In_The_Exception_Is_Correct()
        {
            Assert.Equal("MyKey", ((InvalidExtendedInformationItemException)ExceptionThrown).Key);
        }
    }

    public class When_Removing_Extended_Info_On_Registered_Client_With_Other_Extended_Info_Items : With<Client>
    {
        protected override IEnumerable<IEvent> Given()
        {
            yield return new ClientRegisteredEvent("1234", "http://my.channel.com");
            yield return new ExtendedInformationItemSetEvent("MyKey", "MyValue");
            yield return new ExtendedInformationItemSetEvent("MyOtherKey", "MyOtherValue");
        }

        protected override void When(Client target)
        {
            target.RemoveExtendedInformationItem("MyUnknownKey");
        }

        [Fact]
        public void Then_InvalidExtendedInformationItemException_Is_Thrown()
        {
            Assert.IsType<InvalidExtendedInformationItemException>(ExceptionThrown);
        }

        [Fact]
        public void Then_UniqueId_In_The_Exception_Is_Correct()
        {
            Assert.Equal("1234", ((InvalidExtendedInformationItemException)ExceptionThrown).UniqueId);
        }

        [Fact]
        public void Then_Key_In_The_Exception_Is_Correct()
        {
            Assert.Equal("MyUnknownKey", ((InvalidExtendedInformationItemException)ExceptionThrown).Key);
        }
    }

    public class When_Removing_All_Extended_Infos_On_Registered_Client_With_Extended_Info_Items : With<Client>
    {
        protected override IEnumerable<IEvent> Given()
        {
            yield return new ClientRegisteredEvent("1234", "http://my.channel.com");
            yield return new ExtendedInformationItemSetEvent("MyKey", "MyValue");
            yield return new ExtendedInformationItemSetEvent("MyOtherKey", "MyOtherValue");
        }

        protected override void When(Client target)
        {
            target.RemoveAllExtendedInformationItems();
        }

        [Fact]
        public void Then_AllExtendedInformationItemsRemovedEvent_Is_Sent()
        {
            AssertEvent.IsType<AllExtendedInformationItemsRemovedEvent>(0);
        }

        [Fact]
        public void Then_UniqueId_On_Event_Is_Correct()
        {
            AssertEvent.Contents<AllExtendedInformationItemsRemovedEvent>(0,
                ev => Assert.Equal("1234", ev.UniqueId));
        }
    }

    // ReSharper restore InconsistentNaming
}