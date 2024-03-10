using System.Collections;
using UnityEngine;
using Unity.Notifications;
using System;
using Unity.Notifications.Android;
using UnityEngine.Android;

public class SendPushMessage : MonoBehaviour
{
    protected string titleField = "Title";
    protected string bodyField = "Desctiption push message";
    protected DateTime timeField;
    protected int badgeField = 34895738;

    private IEnumerator Start()
    {

        var group = new AndroidNotificationChannelGroup()
        {
            Id = "Main",
            Name = "Main notifications",
        };
        AndroidNotificationCenter.RegisterNotificationChannelGroup(group);
        var channel = new AndroidNotificationChannel()
        {
            Id = "channel_id",
            Name = "Default Channel",
            Importance = Importance.Default,
            Description = "Generic notifications",
            Group = "Main",  // must be same as Id of previously registered group
        };
        AndroidNotificationCenter.RegisterNotificationChannel(channel);

        yield return RequestNotificationPermission();

        var notification = new AndroidNotification();
        notification.Title = "Your Title";
        notification.Text = "Your Text";
        notification.FireTime = System.DateTime.Now.AddMinutes(0.2);

        var id = AndroidNotificationCenter.SendNotification(notification, "channel_id");
        Debug.Log($"Queued event with ID \"{id}\" at time {notification.FireTime:HH:mm:ss}");

        AndroidNotificationCenter.NotificationReceivedCallback receivedNotificationHandler =
        delegate (AndroidNotificationIntentData data)
        {
            var msg = "Notification received : " + data.Id + "\n";
            msg += "\n Notification received: ";
            msg += "\n .Title: " + data.Notification.Title;
            msg += "\n .Body: " + data.Notification.Text;
            msg += "\n .Channel: " + data.Channel;
            Debug.Log(msg);
        };

        AndroidNotificationCenter.OnNotificationReceived += receivedNotificationHandler;

        //var args = NotificationCenterArgs.Default;
        //args.AndroidChannelId = "default";
        //args.AndroidChannelName = "Notifications";
        //args.AndroidChannelDescription = "Main notifications";
        //NotificationCenter.Initialize(args);

        ////StartCoroutine(RequestPermission());


        //var notification = new Notification()
        //{
        //    Title = "Title text",
        //    Text = "Main text",
        //};

        //var when = System.DateTime.Now.AddMinutes(0.5);
        //int id = NotificationCenter.ScheduleNotification(notification, new NotificationDateTimeSchedule(when));
        //Debug.Log("Notification id: " + id);
        //Debug.Log("When: " + when);
        //NotificationCenter.OnNotificationReceived += NotificationReceived;
    }

    IEnumerator RequestNotificationPermission()
    {
        if (!Permission.HasUserAuthorizedPermission("android.permission.POST_NOTIFICATIONS"))
        {
            Permission.RequestUserPermission("android.permission.POST_NOTIFICATIONS");
        }
        var request = new PermissionRequest();
        while (request.Status == PermissionStatus.RequestPending)
            yield return null;
        Debug.Log("Permission result: " + request.Status);
        // here use request.Status to determine users response
    }

    void NotificationReceived(Notification notification)
    {
        //Debug.Log($"Received notification with title: {notification.Title}");
    }

}
