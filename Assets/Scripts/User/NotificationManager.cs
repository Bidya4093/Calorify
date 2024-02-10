using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Notifications.Android;
using UnityEngine;

public class NotificationManager : MonoBehaviour
{
    // Start is called before the first frame update

    string NOTIFICATION_CHANNEL_ID = "TestId";

    void Start()
    {
        RegisterNotificationChannel(
            channelId: NOTIFICATION_CHANNEL_ID, 
            channelName: "TestChannelName",
            decsription: "Lorem ipsum dolor."
         );

        StartCoroutine(RequestNotificationPermission());

        SendNotification("First time", "Please be gentle with me. It is my first time. Click on me with love.");
        //AndroidNotificationCenter.NotificationReceivedCallback receivedNotificationHandler =
        //delegate (AndroidNotificationIntentData data)
        //{
        //    var msg = "Notification received : " + data.Id + "\n";
        //    msg += "\n Notification received: ";
        //    msg += "\n .Title: " + data.Notification.Title;
        //    msg += "\n .Body: " + data.Notification.Text;
        //    msg += "\n .Channel: " + data.Channel;
        //    Debug.Log(msg);
        //};

        AndroidNotificationCenter.OnNotificationReceived += ReceivedNotificationHandler;
    }

    private void SendNotification(string title, string text)
    {
        var notification = new AndroidNotification();
        notification.Title = title;
        notification.Text = text;
        notification.FireTime = System.DateTime.Now.AddMinutes(0.5);

        int notificationId = AndroidNotificationCenter.SendNotification(notification, NOTIFICATION_CHANNEL_ID);

        Debug.LogFormat("Id: {3}, Title: {0}, Text: {1}, FireTime: {2}", title, text, notification.FireTime, notificationId);

    }

    void ReceivedNotificationHandler(AndroidNotificationIntentData data)
    {
        var msg = "Notification received : " + data.Id + "\n";
        msg += "\n Notification received: ";
        msg += "\n .Title: " + data.Notification.Title;
        msg += "\n .Body: " + data.Notification.Text;
        msg += "\n .Channel: " + data.Channel;
        Debug.Log(msg);
    }

    private void RegisterNotificationChannel(string channelId, string channelName, string decsription, Importance importance = Importance.Default)
    {
        string groupId = "Main";
        // Create channel group with passed parameters
        var group = new AndroidNotificationChannelGroup()
        {
            Id = groupId,
            Name = "Main notifications",
        };
        AndroidNotificationCenter.RegisterNotificationChannelGroup(group);

        // Create channel with passed parameters
        var channel = new AndroidNotificationChannel()
        {
            Id = channelId,
            Name = channelName,
            Importance = importance,
            Description = decsription,
            Group = groupId,  // must be same as Id of previously registered group
        };
        AndroidNotificationCenter.RegisterNotificationChannel(channel);
    }

    IEnumerator RequestNotificationPermission()
    {
        var request = new PermissionRequest();
        while (request.Status == PermissionStatus.RequestPending)
            yield return null;
        // here use request.Status to determine users response

        var notification = new AndroidNotification();
        notification.Title = "Your Title";
        notification.Text = "Your Text";
        notification.FireTime = System.DateTime.Now.AddMinutes(1);

        AndroidNotificationCenter.SendNotification(notification, "channel_id");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
