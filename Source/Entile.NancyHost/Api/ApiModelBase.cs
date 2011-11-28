using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text.RegularExpressions;
using Entile.NancyHost.Api.ViewModel;
using Entile.Server;
using Entile.Server.Commands;

namespace Entile.NancyHost.Api
{

    public abstract class ApiModelBase
    {
        private readonly IMessageDispatcher _commandDispatcher;

        public HttpStatusCode HttpStatusCode { get; protected set; }

        protected ApiModelBase(IMessageDispatcher commandDispatcher)
        {
            HttpStatusCode = HttpStatusCode.OK;

            _commandDispatcher = commandDispatcher;
        }

        protected void DispatchCommand(IMessage command)
        {
            _commandDispatcher.Dispatch(command);
        }

        protected virtual void HandleException(Exception ex)
        {
            HttpStatusCode = HttpStatusCode.InternalServerError;
        }

        protected List<LinkViewModel> ToLinks(Type type, object context)
        {
            var methods = type.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);
            return ToLinks(methods, context);
        }

        protected List<LinkViewModel> ToLinks(IEnumerable<MethodInfo> methodInfos, object context)
        {
            return methodInfos.Select(method => ToLink(method, context)).ToList();
        }

        protected List<LinkViewModel> ToEntrypointLinks(Type type, object context)
        {
            var links = new List<LinkViewModel>();
            var method = type.GetMethod("Self");
            if (method != null)
                links.Add(ToLink(method, context));

            foreach(var m in type.GetMethods())
            {
                var attrs = m.GetCustomAttributes(typeof (ApiMethodAttribute), true).FirstOrDefault() as ApiMethodAttribute;
                if (attrs != null)
                {
                    if (attrs.Entrypoint)
                    {
                        links.Add(ToLink(m, context));
                    }
                }
            }

            return links;
        }

        private LinkViewModel ToLink(MethodInfo method, object context)
        {
            var rawUri = method.GetUri();
            Regex parameters = new Regex(@"\{(?<param>\w+)\}");

            var uri = parameters.Replace(rawUri, m => GetParameter(m, context));

            return new LinkViewModel
            {
                Uri = uri,
                Rel = method.GetRel()
            };
        }

        private string GetParameter(Match m, object context)
        {
            if (context == null)
                return m.Value;

            var paramName = m.Groups["param"].Value;
            var contextType = context.GetType();

            var field = contextType.GetField(paramName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.IgnoreCase);
            if (field != null)
                return field.GetValue(context).ToString();

            return m.Value;
        }
    }
}