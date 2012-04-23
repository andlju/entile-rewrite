using Entile.TestApp.ViewModels;

namespace Entile.TestApp.Actions
{
    public class RegisterClientCommand : ApiQueryBase
    {
        public RegisterClientCommand(IViewContext viewContext, string uri) :
            base(viewContext, uri)
        {
        }

        protected override void OnResponse(string uri, int statusCode, string response)
        {
            
        }
    }

    public class SubscribeCommand : ApiQueryBase
    {
        public SubscribeCommand(IViewContext viewContext, string uri) : base(viewContext, uri)
        {
        }

        protected override void OnResponse(string uri, int statusCode, string response)
        {
            
        }
    }
}