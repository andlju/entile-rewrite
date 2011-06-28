using Newtonsoft.Json.Converters;

namespace Entile.Server
{
    public interface IEventSerializer
    {
        string Serialize(object obj);
        object Deserialize(string json);
    }
}