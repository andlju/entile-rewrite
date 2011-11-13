using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Entile.Server;
using Entile.Server.Commands;
using Entile.Server.Queries;
using Entile.Server.QueryHandlers;
using Entile.Server.ViewHandlers;
using Nancy;

namespace Entile.NancyHost.Api
{
    public static class LinkExtensions
    {
        public static IEnumerable<object> AsLinks(this Type type)
        {
            var methods = type.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);
            return methods.AsLinks();
        }

        public static IEnumerable<object> AsLinks(this IEnumerable<MethodInfo> methodInfos)
        {
            return methodInfos.Select(method => AsLink(method));
        }

        private static object AsLink(this MethodInfo method)
        {
            return new
                       {
                           Uri = method.GetUri(),
                           Rel = method.GetRel()
                       };
        }

        public static string GetResourceName(this Type type)
        {
            var apiAttrib = (ApiAttribute)type.GetCustomAttributes(typeof(ApiAttribute), true).FirstOrDefault();

            string resourceName = type.Name.Replace("ApiModel", string.Empty);
            if (apiAttrib != null)
            {
                resourceName = apiAttrib.ResourceName ?? resourceName;
            }

            return resourceName;
        }

        public static string GetRel(this MethodInfo method)
        {
            var apiMethodAttrib = (ApiMethodAttribute)method.GetCustomAttributes(typeof(ApiMethodAttribute), true).FirstOrDefault();

            string rel = method.Name == "Self" ? method.DeclaringType.GetResourceName() : method.Name;

            if (apiMethodAttrib != null)
            {
                rel = apiMethodAttrib.Rel ?? rel;
            }
            return rel;
        }

        public static string GetBaseUri(this Type type)
        {
            var apiAttrib = (ApiAttribute)type.GetCustomAttributes(typeof (ApiAttribute), true).FirstOrDefault();
            if (apiAttrib == null)
            {
                return "/" + type.GetResourceName();
            }
            return apiAttrib.BaseUri;
        }

        public static string GetUri(this MethodInfo method)
        {
            var apiMethodAttrib = (ApiMethodAttribute)method.GetCustomAttributes(typeof(ApiMethodAttribute), true).FirstOrDefault();

            string baseUri = GetBaseUri(method.DeclaringType);
            string relativeUri = method.Name == "Self" ? "" : "/" + method.Name;

            if (apiMethodAttrib != null)
            {
                relativeUri = apiMethodAttrib.RelativeUri ?? relativeUri;
            }
            return baseUri + relativeUri;
        }

        public static string GetHttpMethod(this MethodInfo method)
        {
            var apiMethodAttrib = (ApiMethodAttribute)method.GetCustomAttributes(typeof(ApiMethodAttribute), true).FirstOrDefault();

            string httpMethod = method.Name == "Self" ? "GET" : "POST";

            if (apiMethodAttrib != null)
            {
                httpMethod = apiMethodAttrib.HttpMethod ?? httpMethod;
            }
            return httpMethod;
        }

        public static object AsForm(this MethodInfo method)
        {
            var httpMethod = method.GetHttpMethod();
            var messageProperties = GetMessageType(method).GetFields();

            return new
                       {
                           Action = httpMethod,
                           Form = messageProperties.ToDictionary(pi => pi.Name, _ => (object)null)
                       };
        }

        public static Type GetMessageType(this MethodInfo method)
        {
            var parameters = method.GetParameters();
            if (parameters.Length != 1 || !typeof(IMessage).IsAssignableFrom(parameters[0].ParameterType))
                return null;

            var messageType = parameters[0].ParameterType;
            return messageType;
        }
    }

    public abstract class ApiModelBase
    {
        private readonly IMessageDispatcher _commandDispatcher;
        private readonly IQueryDispatcher _queryDispatcher;
        private readonly IDictionary<string, object> _apiContext;

        protected ApiModelBase(IMessageDispatcher commandDispatcher, IQueryDispatcher queryDispatcher)
        {
            _commandDispatcher = commandDispatcher;
            _queryDispatcher = queryDispatcher;
            _apiContext = new Dictionary<string, object>();
        }

        protected IMessageDispatcher CommandDispatcher
        {
            get { return _commandDispatcher; }
        }

        protected IQueryDispatcher QueryDispatcher
        {
            get { return _queryDispatcher; }
        }

        protected static IEnumerable<MethodInfo> GetMethodInfos(params Expression<Func<object>>[] methodExpressions)
        {
            foreach (var exp in methodExpressions)
            {
                var bodyType = exp.Body as MethodCallExpression;
                if (bodyType == null)
                    throw new ArgumentException("All arguments to GetMethodInfos must be MethodCallExpressions");

                var method = bodyType.Method;
                yield return method;
            }
            yield break;
        }

    }

    [Api("/")]
    public class RootApiModel : ApiModelBase
    {
        public RootApiModel(IMessageDispatcher commandDispatcher, IQueryDispatcher queryDispatcher)
            : base(commandDispatcher, queryDispatcher)
        {
        }

        public object Self()
        {
            return new
                       {
                           Links = GetMethodInfos(
                               () => new ClientApiModel(null, null).Self(null),
                               () => new ClientApiModel(null, null).Register(null)
                               ).AsLinks()
                       };
        }
    }

    [Api("/client/{clientId}")]
    public class ClientApiModel : ApiModelBase
    {
        public ClientApiModel(IMessageDispatcher commandDispatcher, IQueryDispatcher queryDispatcher)
            : base(commandDispatcher, queryDispatcher)
        {
        }

        public object Self(GetClientQuery query)
        {
            return new
                       {
                           Client = QueryDispatcher.Invoke(query),
                           Links = typeof (ClientApiModel).AsLinks()
                       };
        }

        public object Register(RegisterClientCommand command)
        {
            CommandDispatcher.Dispatch(command);

            return new
                       {
                           Links = typeof (ClientApiModel).AsLinks()
                       };
        }

        public object Unregister(UnregisterClientCommand command)
        {
            CommandDispatcher.Dispatch(command);

            return new
                       {
                           Links = typeof (RootApiModel).AsLinks()
                       };
        }

        public object Subscribe(SubscribeCommand command)
        {
            CommandDispatcher.Dispatch(command);

            return new
                       {
                           Links = typeof (SubscriptionApiModel).AsLinks()
                       };
        }
    }

    [Api("/client/{clientId}/subscription/{subscriptionId}")]
    public class SubscriptionApiModel : ApiModelBase
    {
        public SubscriptionApiModel(IMessageDispatcher commandDispatcher, IQueryDispatcher queryDispatcher)
            : base(commandDispatcher, queryDispatcher)
        {
        }

        public object Unsubscribe(UnsubscribeCommand command)
        {
            CommandDispatcher.Dispatch(command);

            return new
                       {
                           Links = typeof (ClientApiModel).AsLinks()
                       };
        }

    }
}