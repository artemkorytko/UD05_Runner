using System;
using System.Linq;
using Unity.Notifications.Android;
using UnityEngine;

public class NotificationManager : MonoBehaviour
{
    [Serializable]
    public class NotificationItem
    {
        [SerializeField] string _title;
        [SerializeField] string _text;

        public string Title => _title;
        public string Text => _text;
    }

    [SerializeField] NotificationItem[] _notifications;
    [SerializeField] int _repeatListTimes;
    [SerializeField] Color _androidColor;
    [SerializeField] int _hour;
    [SerializeField] int _minute;

#if UNITY_ANDROID
    private void Start()
    {
        try
        {
            AndroidNotificationCenter.CancelAllScheduledNotifications();

            var c = new AndroidNotificationChannel
            {
                Id = "crazy_office_channel",
                Name = "Crazy Office",
                Importance = Importance.Default,
                Description = "Game notifications",
            };
            AndroidNotificationCenter.RegisterNotificationChannel(c);

            var day = 0;
            var rnd = new System.Random();
            var list = _notifications.OrderBy((item) => rnd.Next()).ToArray();
            for (int i = 0; i < _repeatListTimes; i++)
            {
                var notification = new AndroidNotification
                {
                    Title = list[i].Title,
                    Text = list[i].Text,
                    SmallIcon = "icon_0",
                    FireTime = DateTime.Today.AddHours(++day * 24 + _hour).AddMinutes(_minute),
                    Color = _androidColor
                };
                AndroidNotificationCenter.SendNotification(notification, "crazy_office_channel");
            }
        }
        catch
        {
            Debug.LogError(DateTime.Now);
            try
            {
                Debug.LogError(Application.systemLanguage.ToString());
            }
            catch
            {
                Debug.LogError("not find language");
            }
        }
    }
#endif

#if UNITY_IOS
        private void Start()
        {
            var day = 0;
            iOSNotificationCenter.RemoveAllScheduledNotifications();
            iOSNotificationCenter.ApplicationBadge = 0;

            var rnd = new System.Random();
            var list = _notifications.OrderBy((item) => rnd.Next()).ToArray();
            for (int i = 0; i < _repeatListTimes; i++)
            {
                var date = DateTime.Today.AddDays(++day);

                var calendarTrigger = new iOSNotificationCalendarTrigger()
                {
                    Year = date.Year,
                    Month = date.Month,
                    Day = date.Day,
                    Hour = _hour,
                    Minute = _minute,
                    Repeats = false
                };


                var notification = new iOSNotification()
                {
                    Title = list[i].Title,
                    Body = list[i].Text,
                    CategoryIdentifier = "category_a",
                    ThreadIdentifier = "thread1",
                    Trigger = calendarTrigger,
                    Badge = 1,
                    ShowInForeground = false,
                    ForegroundPresentationOption = PresentationOption.None
                };

                if (date < DateTime.Now)
                    continue;

                iOSNotificationCenter.ScheduleNotification(notification);
            }
        }
#endif
}