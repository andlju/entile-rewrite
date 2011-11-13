using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using Entile.NancyHost.Api;
using Entile.Server;
using Entile.Server.Commands;
using Entile.Server.Queries;
using Entile.Server.ViewHandlers;
using Nancy;
using Nancy.Bootstrapper;
using Nancy.ModelBinding;
using Nancy.Responses;
using TinyIoC;

namespace Entile.NancyHost
{
 /*   public class Endpoint
    {
        private string _path;
        private string _rel;
        private string _method;
        private readonly MethodInfo _methodInfo;
        private static Regex _parameters = new Regex(@"\{(?<name>[A-Za-z0-9]*)\}");

        public Endpoint(string baseUri, MethodInfo methodInfo)
        {
            _methodInfo = methodInfo;
            _rel = methodInfo.Name;
            var apiMethodAttribute = (ApiMethodAttribute)(methodInfo.GetCustomAttributes(typeof(ApiMethodAttribute), true).FirstOrDefault());
            if (apiMethodAttribute != null)
            {
                _method = apiMethodAttribute.Method;
                _path = baseUri + apiMethodAttribute.RelativeUri;
            }
            else
            {
                _method = "POST";
                _path = baseUri + "/" + methodInfo.Name;
            }
        }
        
        public MethodInfo MethodInfo { get { return _methodInfo; } }

        public string Path { get { return _path; } }

        public string Method { get { return _method; } }

        private static string Replacer(IDictionary<string, object> parameters, string paramName)
        {
            object val;
            if (parameters.TryGetValue(paramName, out val))
                return val.ToString();
            return "{" + paramName + "}";
        }

        public object ToResponse(IDictionary<string, object> parameters)
        {
            var href = _parameters.Replace(_path, e => Replacer(parameters, e.Groups["name"].Value));

            return new { Href = href , Rel = _rel, Method = _method};
        }
    }
    */
    
    public class CommandBootstrapper : DefaultNancyBootstrapper
    {
        protected override void ConfigureApplicationContainer(TinyIoCContainer container)
        {
            container.Register<IMessageDispatcher>((c, n) => Bootstrapper.CurrentServer.CommandDispatcher);
            container.Register<IQueryDispatcher>((c, n) => Bootstrapper.CurrentServer.QueryDispatcher);
        }
    }
}