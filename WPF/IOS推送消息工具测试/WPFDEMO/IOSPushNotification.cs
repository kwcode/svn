using JdSoft.Apple.Apns.Notifications;
using JHSoft.IOS.pushNotification;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFDEMO
{
    public class IOSPushNotification
    {

        public static void pushNotifications(string strDeviceToken, string strContent)
        {
            bool sandbox = true;
            string deviceToken = strDeviceToken;
            string str2 = "aps_developer_identity2.p12";
            string str3 = "123456";

            //strContent = "{\"userinfo\":{\"name\":\"remote notice\"},";
            //strContent += "\"aps\": {  \"alert\":  {  \"action-loc-key\":\"Open\", \"body\":\"messgae content\" }, \"badge\":1,  \"sound\":\"default\"}}";
            string str4 = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, str2);
            NotificationService service = new NotificationService(sandbox, str4, str3, 1)
            {
                SendRetries = 5,
                ReconnectDelay = 0x1388
            };
            service.Error += new NotificationService.OnError(IOSPushNotification.service_Error);
            service.NotificationTooLong += new NotificationService.OnNotificationTooLong(IOSPushNotification.service_NotificationTooLong);
            service.BadDeviceToken += new NotificationService.OnBadDeviceToken(IOSPushNotification.service_BadDeviceToken);
            service.NotificationFailed += new NotificationService.OnNotificationFailed(IOSPushNotification.service_NotificationFailed);
            service.NotificationSuccess += new NotificationService.OnNotificationSuccess(IOSPushNotification.service_NotificationSuccess);
            service.Connecting += new NotificationService.OnConnecting(IOSPushNotification.service_Connecting);
            service.Connected += new NotificationService.OnConnected(IOSPushNotification.service_Connected);
            service.Disconnected += new NotificationService.OnDisconnected(IOSPushNotification.service_Disconnected);
            Notification notification = new Notification(deviceToken);
            notification.Payload.Alert.Body = strContent;
            notification.Payload.Sound = "default";
            notification.Payload.Badge = 1;
            //notification.Tag = "tag";
            //notification.Payload.HideActionButton = true;
            // notification.Payload.AddCustom("userinfo", new object[] { "{\"name\":\"remote notice\"}" });
            //notification.Payload.CustomItems.Add("userinfo", new object[] { "\"name\":\"remote notice\"" });
            if (service.QueueNotification(notification))
            {
                Console.WriteLine("Notification Queued!");
            }
            else
            {
                Console.WriteLine("Notification Failed to be Queued!");
            }
            service.Close();
            service.Dispose();
        }

        private static void service_BadDeviceToken(object sender, BadDeviceTokenException ex)
        {
            Console.WriteLine("Bad Device Token: {0}", ex.Message);
        }

        private static void service_Connected(object sender)
        {
            Console.WriteLine("Connected...");
        }

        private static void service_Connecting(object sender)
        {
            Console.WriteLine("Connecting...");
        }

        private static void service_Disconnected(object sender)
        {
            Console.WriteLine("Disconnected...");
        }

        private static void service_Error(object sender, Exception ex)
        {
            Console.WriteLine(string.Format("Error: {0}", ex.Message));
        }

        private static void service_NotificationFailed(object sender, Notification notification)
        {
            Console.WriteLine(string.Format("Notification Failed: {0}", notification.ToString()));
        }

        private static void service_NotificationSuccess(object sender, Notification notification)
        {
            Console.WriteLine(string.Format("Notification Success: {0}", notification.ToString()));
        }

        private static void service_NotificationTooLong(object sender, NotificationLengthException ex)
        {
            Console.WriteLine(string.Format("Notification Too Long: {0}", ex.Notification.ToString()));
        }
    }
}
