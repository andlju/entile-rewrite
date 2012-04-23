using Entile.TestApp.ViewModels;

namespace Entile.TestApp.Actions
{
    public class UnsubscribeCommand : ApiQueryBase
    {
        public UnsubscribeCommand(IViewContext viewContext, string uri)
            : base(viewContext, uri)
        {
        }

        protected override void OnResponse(string uri, int statusCode, string response)
        {
            
        }
    }
}