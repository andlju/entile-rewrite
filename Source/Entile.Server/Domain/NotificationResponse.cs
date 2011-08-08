using System;
using System.IO;
using System.Text;
using System.Xml;

namespace Entile.Server.Domain
{
    public interface INotificationMessage
    {
        string AsXml();
    }

    public class NotificationBase : INotificationMessage
    {
        public Guid UniqueNotificationId { get; set; }

        public string AsXml()
        {
            var xmlString = new StringBuilder();
            
            XmlWriterSettings settings = new XmlWriterSettings() { Indent = true };
            XmlWriter writer = XmlWriter.Create(xmlString, settings);
            writer.WriteStartDocument();
            writer.WriteStartElement("wp", "Notification", "WPNotification");

            WriteContent(writer);

            writer.WriteEndElement();
            writer.WriteEndDocument();
            
            writer.Flush();
            
            return xmlString.ToString();
        }

        protected virtual void WriteContent(XmlWriter writer)
        {

        }
    }

    public class ToastNotification : NotificationBase
    {
        public string Title { get; set; }
        public string Body { get; set; }

        public string ParamUri { get; set; }

        protected override void WriteContent(XmlWriter writer)
        {
            writer.WriteStartElement("wp", "Toast");
            
            writer.WriteStartElement("wp", "Text1");
            writer.WriteValue(Title);
            writer.WriteEndElement();

            writer.WriteStartElement("wp", "Text2");
            writer.WriteValue(Body);
            writer.WriteEndElement();

            writer.WriteStartElement("wp", "Param");
            writer.WriteValue(ParamUri);
            writer.WriteEndElement();

            writer.WriteEndElement();
        }
    }

    public class TileNotification : NotificationBase
    {
        public string ParamUri { get; set; }

        public string Title { get; set; }
        public int Counter { get; set; }
        public string BackgroundUri { get; set; }

        public string BackTitle { get; set; }
        public string BackContent { get; set; }
        public string BackBackgroundUri { get; set; }
    }

    public class RawNotification : NotificationBase
    {
        public string RawContent { get; set; }
    }

    public class NotificationResponse
    {
        public int HttpStatusCode { get; set; }
        public string NotificationStatus { get; set; }
        public string SubscriptionStatus { get; set; }
        public string DeviceConnectionStatus { get; set; }

        public NotificationResponse(int httpStatusCode, string notificationStatus, string subscriptionStatus, string deviceConnectionStatus)
        {
            HttpStatusCode = httpStatusCode;
            NotificationStatus = notificationStatus;
            SubscriptionStatus = subscriptionStatus;
            DeviceConnectionStatus = deviceConnectionStatus;
        }
    }
}