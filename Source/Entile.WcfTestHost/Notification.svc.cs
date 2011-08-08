using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using Entile.Server;

namespace Entile.WcfTestHost
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Notifier" in code, svc and config file together.
    public class Notification : INotification
    {
        private readonly INotifier _notifier;

        public Notification()
        {
            _notifier = Bootstrapper.CurrentServer.Notifier;
        }

        public void SendNotificationToAllClients(string title, string body)
        {
            _notifier.SendNotificationToAllClients(title, body);
        }
    }
}
