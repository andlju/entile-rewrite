namespace Entile.TestApp.ViewModels
{
    public class UnsubscribeCommand : ApiCommandBase
    {
        public UnsubscribeCommand(SubscriptionViewModel subscription, string uri)
            : base(subscription.ViewContext, uri)
        {
        }

        protected override void OnResponse(int statusCode, string response)
        {
            
        }
    }
}