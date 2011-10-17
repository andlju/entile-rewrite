using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using Entile.Server;

namespace Entile.WcfTestHost
{

    public class Registration : IRegistration
    {
        private readonly IRegistrator _registrator;

        public Registration()
        {
            _registrator = Bootstrapper.CurrentServer.Registrator;
        }

        public void Register(Guid uniqueId, string notificationChannel)
        {
            _registrator.Register(uniqueId, notificationChannel);
        }

        public void Unregister(Guid uniqueId)
        {
            _registrator.Unregister(uniqueId);
        }
    }
}
