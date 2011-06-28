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

    public class SqlCodeFirstContext : DbContext
    {
        public DbSet<ClientView> ClientViews { get; set; }

        public DbSet<ExtendedInformationView> ExtendedInformationViews { get; set; }
    }

    public class RegistrationViewHandler 
    {

        public void Handle(ClientRegisteredEvent @event)
        {
            using (var context = new SqlCodeFirstContext())
            {
                var clientView = context.ClientViews.Find(@event.UniqueId);
                if (clientView == null)
                {
                    clientView = new ClientView();
                    context.ClientViews.Add(clientView);
                }
                clientView.UniqueId = @event.UniqueId;
                clientView.NotificationChannel = @event.NotificationChannel;
                
                context.SaveChanges();
            }
        }

        public void Handle(ClientUnregisteredEvent @event)
        {
            using (var context = new SqlCodeFirstContext())
            {
                var clientView = context.ClientViews.Find(@event.UniqueId);
                if (clientView != null)
                {
                    context.ClientViews.Remove(clientView);
                }
                context.SaveChanges();
            }
        }
         
    }
}