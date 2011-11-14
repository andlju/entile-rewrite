using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using Entile.Server;

namespace Entile.NancyHost.Api
{
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
}