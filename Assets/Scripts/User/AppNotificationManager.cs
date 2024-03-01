//using NotificationSamples;
using System;
using UnityEngine;
using UnityEngine.UI;
using System.Text;
using Unity.Notifications.Android;
public class AppNotificationManager : MonoBehaviour
{
    [SerializeField, Tooltip("Reference to the notificationAndroidNotificationCenter.")]

 
    private Text notificationScheduledText;

    private const string NOTIFICATION_CHANNEL_ID = "notification_channel_id";
    private const string GAME_NOTIFICATION_CHANNEL_TITLE = "Get back to the game!";
    private const string GAME_NOTIFICATION_CHANNEL_DESCRIPTION = "Notification from my Game";
    private const int DISPLAY_NOTIFICATION_AFTER_DAYS = 0;

    // notification icons
    private string smallIconName = "icon_0";
    private string largeIconName = "icon_1";


    void Start()
{
    InitializeGameChannel();
    //
    ScheduleNotificationForUnactivity();
    //
    DisplayPendingNotification();
}
private void InitializeGameChannel()
{
    var channel = new AndroidNotificationChannel(NOTIFICATION_CHANNEL_ID, GAME_NOTIFICATION_CHANNEL_TITLE, GAME_NOTIFICATION_CHANNEL_DESCRIPTION, Importance.Default);
   AndroidNotificationCenter.RegisterNotificationChannel(channel);
}
private void ScheduleNotificationForUnactivity()
{
    // cancelling old notifications
   AndroidNotificationCenter.CancelAllNotifications();
    //
    ScheduleNotificationForUnactivity(DISPLAY_NOTIFICATION_AFTER_DAYS);
}
private void ScheduleNotificationForUnactivity(int daysIncrement)
{
    string title = GAME_NOTIFICATION_CHANNEL_TITLE;
    string description = GAME_NOTIFICATION_CHANNEL_DESCRIPTION;
        DateTime deliveryTime = DateTime.Now.AddMinutes(1);
    string channel = NOTIFICATION_CHANNEL_ID;
    //
    SendNotification(title, description, deliveryTime, channelId: channel, smallIcon: smallIconName, largeIcon: largeIconName);
}

public void SendNotification(string title, string body, DateTime deliveryTime, int? badgeNumber = null, bool reschedule = false, string channelId = null, string smallIcon = null, string largeIcon = null)
{
        var notification = new AndroidNotification();




        notification.Title = title;
        notification.Text = body;
        notification.Group =
            !string.IsNullOrEmpty(channelId) ? channelId : NOTIFICATION_CHANNEL_ID;
        notification.FireTime = deliveryTime;
        notification.SmallIcon = smallIcon;
        notification.LargeIcon = largeIcon;
        //if (badgeNumber != null)
        //{
        //    notification.BadgeNumber = badgeNumber;
        //}
        //PendingNotification notificationToDisplay = AndroidNotificationCenter.ScheduleNotification(notification);
        //notificationToDisplay.Reschedule = reschedule;
        //
        var id = AndroidNotificationCenter.SendNotification(notification, "channel_id");
        Debug.Log($"Queued notification for unactivity with ID \"{id}\" at time {deliveryTime:dd.MM.yyyy HH:mm:ss}");
}
private void DisplayPendingNotification()
{

        StringBuilder notificationStringBuilder = new StringBuilder("Pending notifications at:");
    notificationStringBuilder.AppendLine();

        var notificationIntentData = AndroidNotificationCenter.GetLastNotificationIntent();
        if (notificationIntentData != null)
        {
            var id = notificationIntentData.Id;
            var channel = notificationIntentData.Channel;
            var notification = notificationIntentData.Notification;
            var Firetime = notification.FireTime;
                    notificationStringBuilder.Append($"{Firetime:dd.MM.yyyy HH:mm:ss}");
                    notificationStringBuilder.AppendLine();
            Debug.Log(notificationStringBuilder.ToString());
        }

        //for (int i =AndroidNotificationCenter.PendingNotifications.Count - 1; i >= 0; --i)
        //{
        //    PendingNotification queuedNotification =AndroidNotificationCenter.PendingNotifications[i];
        //    DateTime? time = queuedNotification.Notification.DeliveryTime;
        //    if (time != null)
        //    {
        //        notificationStringBuilder.Append($"{time:dd.MM.yyyy HH:mm:ss}");
        //        notificationStringBuilder.AppendLine();
        //    }
        //}
        notificationScheduledText.text = notificationStringBuilder.ToString();
}

}

