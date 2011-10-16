using System.Linq;
using Entile.Server.Events;

namespace Entile.Server.ViewHandlers
{
    public class ExtendedInformationViewHandler : 
        IMessageHandler<ExtendedInformationItemSetEvent>,
        IMessageHandler<ExtendedInformationItemRemovedEvent>,
        IMessageHandler<AllExtendedInformationItemsRemovedEvent>,
        IMessageHandler<ClientUnregisteredEvent>
    {
        public void Handle(ExtendedInformationItemSetEvent command)
        {
            var idStr = command.AggregateId.ToString();
            using (var context = new EntileViews())
            {
                var item = context.ExtendedInformationViews.Find(command.AggregateId, command.Key);
                if (item == null)
                {
                    item = new ExtendedInformationView() { ClientViewUniqueId = idStr, Key = command.Key };
                    context.ExtendedInformationViews.Add(item);
                }
                item.Value = command.Value;
                context.SaveChanges();
            }
        }

        public void Handle(ExtendedInformationItemRemovedEvent command)
        {
            var idStr = command.AggregateId.ToString();
            using (var context = new EntileViews())
            {
                var item = context.ExtendedInformationViews.Find(idStr, command.Key);
                if (item != null)
                {
                    context.ExtendedInformationViews.Remove(item);
                }
                context.SaveChanges();
            }
        }

        public void Handle(AllExtendedInformationItemsRemovedEvent command)
        {
            var idStr = command.AggregateId.ToString();
            using (var context = new EntileViews())
            {
                var items = from i in context.ExtendedInformationViews
                            where i.ClientViewUniqueId == idStr
                            select i;
                foreach (var item in items)
                {
                    context.ExtendedInformationViews.Remove(item);
                }
                context.SaveChanges();
            }
        }

        public void Handle(ClientUnregisteredEvent command)
        {
            var idStr = command.AggregateId.ToString();
            using (var context = new EntileViews())
            {
                var items = from i in context.ExtendedInformationViews
                            where i.ClientViewUniqueId == idStr
                            select i;
                foreach (var item in items)
                {
                    context.ExtendedInformationViews.Remove(item);
                }
                context.SaveChanges();
            }
        }
    }
}