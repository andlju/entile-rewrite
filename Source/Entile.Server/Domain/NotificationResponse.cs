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

    public abstract class NotificationBase : INotificationMessage
    {
        public Guid NotificationId { get; private set; }

        protected NotificationBase(Guid notificationId)
        {
            NotificationId = notificationId;
        }

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

        protected abstract void WriteContent(XmlWriter writer);
    }

    public class ToastNotification : NotificationBase
    {
        public string Title { get; private set; }
        public string Body { get; private set; }

        public string ParamUri { get; private set; }

        public ToastNotification(Guid notificationId, string title, string body, string paramUri) : base(notificationId)
        {
            Title = title;
            Body = body;
            ParamUri = paramUri;
        }

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
        public string Title { get; private set; }
        public int Counter { get; private set; }
        public string BackgroundUri { get; private set; }

        public string BackTitle { get; private set; }
        public string BackContent { get; private set; }
        public string BackBackgroundUri { get; private set; }

        public string ParamUri { get; private set; }

        public TileNotification(Guid notificationId, string title, int counter, string backgroundUri, string backTitle, string backContent, string backBackgroundUri, string paramUri) : base(notificationId)
        {
            Title = title;
            Counter = counter;
            BackgroundUri = backgroundUri;
            BackTitle = backTitle;
            BackContent = backContent;
            BackBackgroundUri = backBackgroundUri;
            ParamUri = paramUri;
        }

        protected override void WriteContent(XmlWriter writer)
        {
            throw new NotImplementedException();
        }
    }

    public class RawNotification : NotificationBase
    {
        public string RawContent { get; private set; }

        public RawNotification(Guid notificationId, string rawContent) : base(notificationId)
        {
            RawContent = rawContent;
        }

        protected override void WriteContent(XmlWriter writer)
        {
            throw new NotImplementedException();
        }
    }

    public class NotificationResponse
    {
        public int HttpStatusCode { get; set; }
        public string NotificationStatus { get; set; }
        public string DeviceConnectionStatus { get; set; }
        public string SubscriptionStatus { get; set; }

        public NotificationResponse(int httpStatusCode, string notificationStatus, string deviceConnectionStatus, string subscriptionStatus)
        {
            HttpStatusCode = httpStatusCode;
            NotificationStatus = notificationStatus;
            DeviceConnectionStatus = deviceConnectionStatus;
            SubscriptionStatus = subscriptionStatus;
        }
    }
}