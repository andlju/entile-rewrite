using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Windows.Input;

namespace Entile.TestApp.ViewModels
{
    public class ApiCommandBase : ICommand
    {
        private static readonly Regex ParametersRegex = new Regex(@"\{(?<param>\w+)\}");

        private IViewContext _viewContext;
        private string _uri;
        private string[] _params;

        public ApiCommandBase(IViewContext viewContext, string uri)
        {
            _viewContext = viewContext;
            _uri = uri;
            _params = ParametersRegex.Matches(_uri).Cast<Match>().Select(m => m.Groups["param"].Value).ToArray();
        }

        public bool CanExecute(object parameter)
        {
            return _params.All(p => _viewContext.GetValue(p) != null);
        }

        public void Execute(object parameter)
        {
            var actualUri = ParametersRegex.Replace(_uri, m => _viewContext.GetValue(m.Groups["param"].Value).ToString());

            HttpWebRequest request = WebRequest.CreateHttp(actualUri);
            request.Accept = "application/json";
            // if (request.Method != "GET")
            // request.ContentType = "application/json";

            request.BeginGetResponse(ReadCallback, request);
        }

        private void ReadCallback(IAsyncResult asynchronousResult)
        {
            var request = (HttpWebRequest)asynchronousResult.AsyncState;

            var response = (HttpWebResponse)request.EndGetResponse(asynchronousResult);
            
            string content;
            using (var streamReader = new StreamReader(response.GetResponseStream()))
            {
                content = streamReader.ReadToEnd();
            }

            OnResponse((int)response.StatusCode, content);
        }

        public event EventHandler CanExecuteChanged;

        protected virtual void OnResponse(int statusCode, string response)
        {
            
        }
    }
}