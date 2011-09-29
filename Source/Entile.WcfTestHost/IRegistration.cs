using System;
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
        void Register(Guid uniqueId, string notificationChannel);

        [OperationContract]
        void Unregister(Guid uniqueId);

        [OperationContract]
        void SetExtendedInformationItem(Guid uniqueId, string key, string value);

        [OperationContract]
        void RemoveExtendedInformationItem(Guid uniqueId, string key);

        [OperationContract]
        void RemoveAllExtendedInformationItems(Guid uniqueId);
    }
}
