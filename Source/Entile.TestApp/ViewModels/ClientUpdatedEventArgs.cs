using System;
using Entile.TestApp.Models;

namespace Entile.TestApp.ViewModels
{
    public class ClientUpdatedEventArgs : EventArgs
    {
        public ClientModel Client { get; private set; }

        public ClientUpdatedEventArgs(ClientModel client)
        {
            Client = client;
        }
    }
}