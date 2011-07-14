﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace Entile.WcfTestHost
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IRegistration" in both code and config file together.
    [ServiceContract]
    public interface IRegistration
    {
        [OperationContract]
        void Register(string uniqueId, string notificationChannel);

        [OperationContract]
        void Unregister(string uniqueId);
    }
}