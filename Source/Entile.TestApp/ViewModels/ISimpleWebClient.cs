using System;

namespace Entile.TestApp.ViewModels
{
    public interface ISimpleWebClient
    {
        void SendRequest(string uri, string method, string content);
        event EventHandler<WebResponseEventArgs> ResponseCompleted;
    }
}