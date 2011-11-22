using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Entile.Server;
using Entile.Server.Commands;

namespace Entile.NancyHost.Api
{

    public abstract class ApiModelBase
    {
        private readonly IMessageDispatcher _commandDispatcher;

        public Dictionary<string, object> Context { get; private set; }

        public HttpStatusCode HttpStatusCode { get; protected set; }

        protected ApiModelBase(IMessageDispatcher commandDispatcher)
        {
            HttpStatusCode = HttpStatusCode.OK;
            Context = new Dictionary<string, object>();

            _commandDispatcher = commandDispatcher;
        }

        protected void DispatchCommand(IMessage command)
        {
            try
            {
                _commandDispatcher.Dispatch(command);
            } 
            catch(Exception ex)
            {
                HandleException(ex);
            }
        }

        protected virtual void HandleException(Exception ex)
        {
            HttpStatusCode = HttpStatusCode.InternalServerError;
        }

        protected IEnumerable<object> ToLinks(Type type)
        {
            var methods = type.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);
            return ToLinks(methods);
        }

        protected IEnumerable<object> ToLinks(IEnumerable<MethodInfo> methodInfos)
        {
            return methodInfos.Select(method => ToLink(method)).ToArray();
        }

        protected IEnumerable<object> ToEntrypointLinks(Type type)
        {
            var method = type.GetMethod("Self");
            return new[] { ToLink(method) };
        }

        private object ToLink( MethodInfo method)
        {
            return new
            {
                Uri = method.GetUri(),
                Rel = method.GetRel()
            };
        }

    }
}