using System;
using System.IO;
using System.Net;
using Entile.TestApp.ViewModels;

namespace Entile.TestApp.Utilities
{
    public class SimpleWebClient : ISimpleWebClient
    {
        public void SendRequest(string uri, string method, string content)
        {
            HttpWebRequest request = WebRequest.CreateHttp(uri);
            request.Accept = "application/json";
            request.Method = method;
            if (request.Method != "GET")
            {
                request.ContentType = "application/json";
                request.BeginGetRequestStream(ar =>
                                                  {
                                                      using (var stream = ((HttpWebRequest)ar.AsyncState).EndGetRequestStream(ar))
                                                      using (var writer = new StreamWriter(stream))
                                                      {
                                                          writer.Write(content);
                                                      }
                                                  },
                                              request);
            }

            request.BeginGetResponse(ReadCallback, request);
        }

        private void ReadCallback(IAsyncResult asynchronousResult)
        {
            var request = (HttpWebRequest)asynchronousResult.AsyncState;

            HttpWebResponse response;
            try
            {
                response = (HttpWebResponse)request.EndGetResponse(asynchronousResult);

            }
            catch (WebException ex)
            {
                response = (HttpWebResponse)ex.Response;
            }

            string content;
            using (var streamReader = new StreamReader(response.GetResponseStream()))
            {
                content = streamReader.ReadToEnd();
            }

            if (ResponseCompleted != null)
            {
                ResponseCompleted(this, new WebResponseEventArgs(request.RequestUri.ToString(), (int)response.StatusCode, content));
            }
        }

        public event EventHandler<WebResponseEventArgs> ResponseCompleted;
    }
}