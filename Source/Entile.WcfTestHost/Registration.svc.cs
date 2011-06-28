using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using Entile.Server;

namespace Entile.WcfTestHost
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Registration" in code, svc and config file together.
    public class Registration : IRegistration
    {
        private readonly IRegistrator _registrator;

        public Registration()
        {
            _registrator = EntileBootstrapper.CreateRegistrator();
        }

        public void Register(string uniqueId, string notificationChannel)
        {
            _registrator.Register(uniqueId, notificationChannel);
        }

        public void Unregister(string uniqueId)
        {
            _registrator.Unregister(uniqueId);
        }
    }
}
