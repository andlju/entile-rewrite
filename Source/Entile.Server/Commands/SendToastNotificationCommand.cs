using System;
using System.ComponentModel.DataAnnotations;

namespace Entile.Server.Commands
{
    public class SendToastNotificationCommand : CommandBase
    {
        [Key]
        public Guid ClientId { get; set; }
        [Key]
        public Guid SubscriptionId { get; set; }
        [Key]
        public Guid NotificationId { get; set; }

        [Required]
        [Display(Description = "Title of the Toast")]
        public string Title { get; set; }

        [Display(Description = "Body of the Toast")]
        public string Body { get; set; }

        [Required]
        [Display(Description = "Number of attempts")]
        public int NumberOfAttempts { get; set; }

        public SendToastNotificationCommand()
        {
            
        }

        public SendToastNotificationCommand(Guid clientId, Guid subscriptionId, Guid notificationId, string title, string body, int numberOfAttempts)
        {
            ClientId = clientId;
            SubscriptionId = subscriptionId;
            NotificationId = notificationId;
            Title = title;
            Body = body;
            NumberOfAttempts = numberOfAttempts;
        }
    }
}