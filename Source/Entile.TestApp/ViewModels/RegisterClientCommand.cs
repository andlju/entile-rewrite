namespace Entile.TestApp.ViewModels
{
    public class RegisterClientCommand : ApiCommandBase
    {
        public RegisterClientCommand(EntileViewModel viewModel, string uri) :
            base(viewModel.ViewContext, uri)
        {
        }

        protected override void OnResponse(int statusCode, string response)
        {
            
        }
    }
}