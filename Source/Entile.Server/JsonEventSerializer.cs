using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Entile.Server
{
    public class JsonEventSerializer : IEventSerializer
    {
        private Dictionary<string, Type> _knownTypes = new Dictionary<string, Type>();

        public void RegisterKnownEventType<T>()
        {
            var type = typeof(T);
            _knownTypes[type.Name] = type;
        }

        public string Serialize(object obj)
        {
            var json = new JObject(new JProperty(obj.GetType().Name, JObject.FromObject(obj)));
            
            return json.ToString(Formatting.None);
        }

        public object Deserialize(string json)
        {
            var jObj = JObject.Parse(json);
            var prop = jObj.Properties().First();
            var typeName = prop.Name;
            Type type;

            if (!_knownTypes.TryGetValue(typeName, out type))
                throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, "Serializer doesn't know the event type {0}", typeName));

            return JsonConvert.DeserializeObject(prop.Value.ToString(Formatting.None), type);
        }
    }
}