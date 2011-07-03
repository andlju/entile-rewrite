using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using Entile.Server.Events;

namespace Entile.Server.ViewHandlers
{
    public class ClientView
    {
        [Key]
        public string UniqueId { get; set; }

        public string NotificationChannel { get; set; }
    }

    public class ExtendedInformationView
    {
        [Key]
        [Column(Order = 0)]
        public string ClientViewUniqueId { get; set; }

        [Key]
        [Column(Order = 1)]
        public string Key { get; set; }

        public string Value { get; set; }

        public ClientView ClientView { get; set; }
    }

    public class EntileViews : DbContext
    {
        public DbSet<ClientView> ClientViews { get; set; }

        public DbSet<ExtendedInformationView> ExtendedInformationViews { get; set; }
    }

    public class RegistrationViewHandler 
    {

        public void Handle(ClientRegisteredEvent @event)
        {
            using (var context = new EntileViews())
            {
                var clientView = new ClientView
                                     {
                                         UniqueId = @event.UniqueId, 
                                         NotificationChannel = @event.NotificationChannel
                                     };

                context.ClientViews.Add(clientView);
                
                context.SaveChanges();
            }
        }

        public void Handle(ClientUnregisteredEvent @event)
        {
            using (var context = new EntileViews())
            {
                var clientView = context.ClientViews.Find(@event.UniqueId);

                context.ClientViews.Remove(clientView);
                
                context.SaveChanges();
            }
        }

        public void Handle(ClientRegistrationUpdatedEvent @event)
        {
            using (var context = new EntileViews())
            {
                var clientView = context.ClientViews.Find(@event.UniqueId);
                clientView.NotificationChannel = @event.NotificationChannel;

                context.SaveChanges();
            }
        }
         
    }
}