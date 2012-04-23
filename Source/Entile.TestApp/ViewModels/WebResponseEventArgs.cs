using System;

namespace Entile.TestApp.ViewModels
{
    public class WebResponseEventArgs : EventArgs
    {
        public WebResponseEventArgs(string uri, int statusCode, string content)
        {
            Uri = uri;
            Content = content;
            StatusCode = statusCode;
        }

        public string Uri { get; private set; }
        public string Content { get; private set; }
        public int StatusCode { get; private set; }
    }
}