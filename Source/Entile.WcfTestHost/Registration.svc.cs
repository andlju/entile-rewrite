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

        public void SetExtendedInformationItem(Guid uniqueId, string key, string value)
        {
            _registrator.SetExtendedInformation(uniqueId, key, value);
        }

        public void RemoveExtendedInformationItem(Guid uniqueId, string key)
        {
            _registrator.RemoveExtendedInformation(uniqueId, key);
        }

        public void RemoveAllExtendedInformationItems(Guid uniqueId)
        {
            _registrator.RemoveAllExtendedInformation(uniqueId);
        }
    }
}
