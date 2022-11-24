using System;
using UnityEngine;
using Unity.Notifications.Android;
using System.Collections.Generic;
using UnityEditor;

namespace TeraJet
{
    public class NotificationHandler
    {
        static List<string> Handled_Ids = new List<string>();

        static string _Channel_Id = Application.identifier + "_notify_daily_reminder";
        static string _Icon_Small = "notify_icon_small"; //this is setup under Project Settings -> Mobile Notifications
        static string _Icon_Large = "notify_icon_large"; //this is setup under Project Settings -> Mobile Notifications
        static string _Channel_Title = "Daily Reminders";
        static string _Channel_Description = "Get daily updates to see anything you missed.";

        static string m_Title = "We Miss You!";
        static string m_Desc = "Come Back! Come Back! Come Back!";

        public static void NotificationInitialize()
        {
            Debug.Log("NotificationManager: Start");

            TeraGameConfig teraGameConfig = GameUtils.LoadSDKSettings();
            if (teraGameConfig != null)
            {
                m_Title = teraGameConfig.notificationTitle;
                m_Desc = teraGameConfig.notificationDesc;
            }
            //always remove any currently displayed notifications
            Unity.Notifications.Android.AndroidNotificationCenter.CancelAllDisplayedNotifications();

            //check if this was openened from a notification click
            var notification_intent_data = AndroidNotificationCenter.GetLastNotificationIntent();

            //this is just for debugging purposes
            if (notification_intent_data != null)
            {
                Debug.Log("notification_intent_data.Id: " + notification_intent_data.Id);
                Debug.Log("notification_intent_data.Channel: " + notification_intent_data.Channel);
                Debug.Log("notification_intent_data.Notification: " + notification_intent_data.Notification);
            }


            //if the notification intent is not null and we have not already seen this notification id, do something
            //using a static List to store already handled notification ids
            if (notification_intent_data != null && Handled_Ids.Contains(notification_intent_data.Id.ToString()) == false)
            {
                Handled_Ids.Add(notification_intent_data.Id.ToString());
                return;
            }
            else
            {
                Debug.Log("notification_intent_data is null or already handled");
            }



            //dont do anything further if the user has disabled notifications
            //this assumes you have additional ui to enabled/disable this preference
            var allow_notifications = PlayerPrefs.GetString("notifications");
            if (allow_notifications?.ToLower() == "false")
            {
                Debug.Log("Notifications Disabled");
                return;
            }


            Setup_Notifications();
        }


        static void Setup_Notifications()
        {
            Debug.Log("NotificationsManager: Setup_Notifications");


            //initialize the channel
            Initialize();


            //schedule the next notification
            Schedule_Daily_Reminder();
        }


        static void Initialize()
        {
            Debug.Log("NotificationManager: Initialize");

            var androidChannel = new AndroidNotificationChannel(_Channel_Id, _Channel_Title, _Channel_Description, Importance.Default);
            AndroidNotificationCenter.RegisterNotificationChannel(androidChannel);
        }



        static void Schedule_Daily_Reminder()
        {
            Debug.Log("NotificationManager: Schedule_Daily_Reminder");


            //since this is the only notification I have, I will cancel any currently pending notifications
            //if I create more types of notifications, additional logic will be needed
            AndroidNotificationCenter.CancelAllScheduledNotifications();


            //create new schedule
            string title = m_Title;
            string body = m_Desc;

            //show at the specified time - 10:30 AM
            //you could also always set this a certain amount of hours ahead, since this code resets the schedule, this could be used to prompt the user to play again if they haven't played in a while
            DateTime delivery_time = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 10, 30, 0);
            if (delivery_time < DateTime.Now)
            {
                //if in the past (ex: this code runs at 11:00 AM), push delivery date forward 1 day
                delivery_time = delivery_time.AddDays(1);
            }
            else if ((delivery_time - DateTime.Now).TotalHours <= 0)
            {
                //optional
                //if too close to current time (<= 4 hours away), push delivery date forward 1 day
                delivery_time = delivery_time.AddDays(1);
            }
            Debug.Log("Delivery Time: " + delivery_time.ToString());


            //you currently do not need the notification id
            //if you had multiple notifications, you could store this and use it to cancel a specific notification
            var scheduled_notification_id = AndroidNotificationCenter.SendNotification(
                new AndroidNotification()
                {
                    Title = title,
                    Text = body,
                    FireTime = delivery_time,
                    SmallIcon = _Icon_Small,
                    LargeIcon = _Icon_Large
                },
                _Channel_Id);
        }
    }
}
