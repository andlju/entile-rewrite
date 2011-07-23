using System.Linq;
using Entile.Server.Events;

namespace Entile.Server.ViewHandlers
{
    public class ExtendedInformationViewHandler : 
        IMessageHandler<ExtendedInformationItemSetEvent>,
        IMessageHandler<ExtendedInformationItemRemovedEvent>,
        IMessageHandler<AllExtendedInformationItemsRemovedEvent>
    {
        public void Handle(ExtendedInformationItemSetEvent command)
        {
            using (var context = new EntileViews())
            {
                var item = context.ExtendedInformationViews.Find(command.UniqueId, command.Key);
                if (item == null)
                {
                    item = new ExtendedInformationView() { ClientViewUniqueId = command.UniqueId, Key = command.Key};
                    context.ExtendedInformationViews.Add(item);
                }
                item.Value = command.Value;
                context.SaveChanges();
            }
        }

        public void Handle(ExtendedInformationItemRemovedEvent command)
        {
            using (var context = new EntileViews())
            {
                var item = context.ExtendedInformationViews.Find(command.UniqueId, command.Key);
                if (item != null)
                {
                    context.ExtendedInformationViews.Remove(item);
                }
                context.SaveChanges();
            }
        }

        public void Handle(AllExtendedInformationItemsRemovedEvent command)
        {
            using (var context = new EntileViews())
            {
                var items = from i in context.ExtendedInformationViews
                            where i.ClientViewUniqueId == command.UniqueId
                            select i;
                foreach(var item in items)
                {
                    context.ExtendedInformationViews.Remove(item);
                }
                context.SaveChanges();
            }
        }
    }
}