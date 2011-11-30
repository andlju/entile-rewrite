using System.IO;
using Newtonsoft.Json;

namespace Entile.TestApp.ViewModels
{
    public class RootApiCommand : ApiCommandBase
    {
        private readonly EntileViewModel _viewModel;

        public RootApiCommand(EntileViewModel viewModel, string uri)
            : base(viewModel.ViewContext, uri)
        {
            _viewModel = viewModel;
        }

        protected override void OnResponse(int statusCode, string response)
        {
            var jsonSerializer = new JsonSerializer();
            using (var reader = new JsonTextReader(new StringReader(response)))
            {
                // Read to Links
                while (reader.Read())
                {
                    if (reader.TokenType == JsonToken.PropertyName && reader.Value.ToString() == "Links")
                    {
                        reader.Read();
                        break;
                    }
                }
                var links = jsonSerializer.Deserialize<LinkModel[]>(reader);
                _viewModel.LoadLinks(links);
            }
        }
    }
}