using Newtonsoft.Json.Converters;

namespace Entile.Server
{
    public interface IMessageSerializer
    {
        string Serialize(object obj);
        object Deserialize(string json);
    }
}