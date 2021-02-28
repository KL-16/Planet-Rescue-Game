using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Notifications.Android;

public class NotificationManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        AndroidNotificationCenter.CancelAllNotifications();

        var channel = new AndroidNotificationChannel()
        {
            Id = "channel_id",
            Name = "Default Channel",
            Importance = Importance.Default,
            Description = "Generic notifications",
        };
        AndroidNotificationCenter.RegisterNotificationChannel(channel);

        var notification = new AndroidNotification
        {
            Title = "Helped the environment today?",
            Text = "Have some fun and contribute to environmental protection",
            FireTime = System.DateTime.Now.AddHours(24),
            LargeIcon = "icon_large"
        };

        AndroidNotificationCenter.SendNotification(notification, "channel_id");

    }

}
