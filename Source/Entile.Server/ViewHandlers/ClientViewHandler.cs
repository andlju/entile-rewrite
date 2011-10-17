using System;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using Entile.Server.Events;

namespace Entile.Server.ViewHandlers
{
    public class ClientView
    {
        [Key]
        public Guid ClientId { get; set; }

        public string NotificationChannel { get; set; }

        public Collection<SubscriptionView> Subscriptions { get; set; }
    }

    public class SubscriptionView
    {
        [Key]
        [Column(Order = 0)]
        public Guid ClientId { get; set; }

        [Key]
        [Column(Order = 1)]
        public Guid SubscriptionId { get; set; }

        public int NotificationKind { get; set; }
        public string ParamUri { get; set; }

        public Collection<ExtendedInformationView> ExtendedInformation { get; set; }
    }

    public class ExtendedInformationView
    {
        [Key]
        [Column(Order = 0)]
        public Guid ClientId { get; set; }

        [Key]
        [Column(Order = 1)]
        public Guid SubscriptionId { get; set; } 

        [Key]
        [Column(Order = 2)]
        public string Key { get; set; }

        public string Value { get; set; }
    }

    public class EntileViews : DbContext
    {
        public EntileViews() : base("EntileViews")
        {
        }
        
        public DbSet<ClientView> ClientViews { get; set; }
    }

    public class ClientViewHandler :
        IMessageHandler<ClientRegisteredEvent>,
        IMessageHandler<ClientRegistrationUpdatedEvent>,
        IMessageHandler<ClientUnregisteredEvent>
    {

        public void Handle(ClientRegisteredEvent @event)
        {
            var idStr = @event.AggregateId;
            using (var context = new EntileViews())
            {
                var clientView = context.ClientViews.Find(idStr);
                if (clientView == null)
                {
                    clientView = new ClientView
                    {
                        ClientId = idStr,
                    };

                    context.ClientViews.Add(clientView);
                }
                clientView.NotificationChannel = @event.NotificationChannel;
                context.SaveChanges();
            }
        }

        public void Handle(ClientUnregisteredEvent @event)
        {
            using (var context = new EntileViews())
            {
                var clientView = context.ClientViews.Include("Subscriptions.ExtendedInformation").Where(c => c.ClientId == @event.AggregateId).FirstOrDefault();
                if (clientView != null)
                {
                    context.ClientViews.Remove(clientView);
                    context.SaveChanges();
                }
            }
        }

        public void Handle(ClientRegistrationUpdatedEvent @event)
        {
            var idStr = @event.AggregateId;
            using (var context = new EntileViews())
            {
                var clientView = context.ClientViews.Find(idStr);
                if (clientView == null)
                {
                    clientView = new ClientView
                    {
                        ClientId = idStr,
                    };

                    context.ClientViews.Add(clientView);
                }
                clientView.NotificationChannel = @event.NotificationChannel;
                context.SaveChanges();
            }
        }
         
    }
}