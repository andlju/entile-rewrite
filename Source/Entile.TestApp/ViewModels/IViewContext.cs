using System.Collections.Generic;

namespace Entile.TestApp.ViewModels
{
    public interface IViewContext
    {
        object GetValue(string key);
    }

    public class EntileViewContext : IViewContext
    {
        private readonly IViewContext _parentContext;

        public EntileViewContext()
        {
        }

        public EntileViewContext(IViewContext parentContext)
        {
            _parentContext = parentContext;
        }

        private Dictionary<string, object> _lookup = new Dictionary<string, object>();

        public object GetValue(string key)
        {
            object val;
            key = key.ToLowerInvariant();
            if (_lookup.TryGetValue(key, out val))
                return val;
            if (_parentContext != null)
                return _parentContext.GetValue(key);

            return null;
        }

        public void SetValue(string key, object value)
        {
            _lookup[key.ToLowerInvariant()] = value;
        }
    }

}