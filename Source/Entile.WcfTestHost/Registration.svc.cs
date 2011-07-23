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

        public void Register(string uniqueId, string notificationChannel)
        {
            _registrator.Register(uniqueId, notificationChannel);
        }

        public void Unregister(string uniqueId)
        {
            _registrator.Unregister(uniqueId);
        }

        public void SetExtendedInformationItem(string uniqueId, string key, string value)
        {
            _registrator.SetExtendedInformation(uniqueId, key, value);
        }

        public void RemoveExtendedInformationItem(string uniqueId, string key)
        {
            _registrator.RemoveExtendedInformation(uniqueId, key);
        }

        public void RemoveAllExtendedInformationItems(string uniqueId)
        {
            _registrator.RemoveAllExtendedInformation(uniqueId);
        }
    }
}
