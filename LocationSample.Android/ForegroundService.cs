using System;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Support.V4.App;
using Xamarin.Forms;
using static Android.OS.PowerManager;
using AndroidApp = Android.App.Application;

namespace LocationSample.Droid
{
    [Service]
    public class ForegroundService : Service
    {
        ILocationHelper locationHelper;
        const string channelId = "default";
        const string channelName = "Default";
        const string channelDescription = "The default channel for notifications.";
        const int pendingIntentId = 0;
        public event EventHandler NotificationReceived;

        public const string TitleKey = "title";
        public const string MessageKey = "message";

        bool channelInitialized = false;
        Android.App.NotificationManager manager;
        public ForegroundService()
        {
            locationHelper = DependencyService.Get<ILocationHelper>();
        }

        public override void OnCreate()
        {
            base.OnCreate();

            if (Build.VERSION.SdkInt >= BuildVersionCodes.O)
            { 
                StartForeground(1, ScheduleNotification("Location","Running..."));
            } 
        }

        public Notification ScheduleNotification(string title, string message)
        {
            if (!channelInitialized)
            {
                CreateNotificationChannel();
            }
            Intent intent = new Intent(AndroidApp.Context, typeof(MainActivity));
            intent.PutExtra(TitleKey, title);
            intent.PutExtra(MessageKey, message);

            Android.App.PendingIntent pendingIntent = Android.App.PendingIntent.GetActivity(AndroidApp.Context, pendingIntentId, intent, Android.App.PendingIntentFlags.OneShot);

            NotificationCompat.Builder builder = new NotificationCompat.Builder(AndroidApp.Context, channelId)
                .SetContentIntent(pendingIntent)
                .SetContentTitle(title)
                .SetContentText(message)
                .SetLargeIcon(BitmapFactory.DecodeResource(AndroidApp.Context.Resources, Resource.Drawable.abc_action_bar_item_background_material))
                .SetSmallIcon(Resource.Drawable.abc_btn_check_material)
                .SetOngoing(true);

            var notification = builder.Build();

            return notification;
        }
        public void ReceiveNotification(string title, string message)
        {
            var args = new NotificationEventArgs()
            {
                Title = title,
                Message = message,
            };
            NotificationReceived?.Invoke(null, args);
        }

        void CreateNotificationChannel()
        {
            manager = (Android.App.NotificationManager)AndroidApp.Context.GetSystemService(AndroidApp.NotificationService);

            if (Build.VERSION.SdkInt >= BuildVersionCodes.O)
            {
                var channelNameJava = new Java.Lang.String(channelName);
                var channel = new Android.App.NotificationChannel(channelId, channelNameJava, Android.App.NotificationImportance.Default)
                {
                    Description = channelDescription
                };
                manager.CreateNotificationChannel(channel);
            }

            channelInitialized = true;
        }
        public override StartCommandResult OnStartCommand(Intent intent, StartCommandFlags flags, int startId)
        {
            base.OnStartCommand(intent, flags, startId);

 
            locationHelper.Start();
            return StartCommandResult.Sticky;
        }

 
        public override void OnDestroy()
        {
            base.OnDestroy();
            StopSelf();
        }

        public override IBinder OnBind(Intent intent)
        {
            return null;
        }
    }
}
