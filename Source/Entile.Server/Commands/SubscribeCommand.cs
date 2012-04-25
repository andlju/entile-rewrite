using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Entile.Server.Domain;

namespace Entile.Server.Commands
{
    public class SubscribeCommand : CommandBase
    {
        [Key]
        public Guid ClientId { get; set; }
        
        [Key]
        public Guid SubscriptionId { get; set; }
        
        [Required]
        [Display(Description = "Notification kind")]
        public NotificationKind Kind { get; set; }

        [Required]
        [Display(Description = "Parameter uri")]
        public string Uri { get; set; }

        [Display(Description = "Extended information")]
        public Dictionary<string, string> ExtendedInformation { get; set; }

        public SubscribeCommand()
        {

        }

        public SubscribeCommand(Guid clientId, Guid subscriptionId, NotificationKind kind, string uri, Dictionary<string, string> extendedInformation)
        {
            ClientId = clientId;
            SubscriptionId = subscriptionId;
            Kind = kind;
            Uri = uri;
            ExtendedInformation = extendedInformation;
        }
    }
}