using System;
using Android;
using Android.Content.PM;
using Android.Locations;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Support.V4.App;
using Android.Support.V4.Content;
using Android.Support.V7.App;
using Android.Util;
using Android.Views;
using Android.Widget;
using LocationSample.Droid;
using Xamarin.Forms;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Support.V4.App;
using Java.Sql;
using Xamarin.Forms;
using static Android.OS.PowerManager;
using AndroidApp = Android.App.Application;
[assembly: Dependency(typeof(LocationListener))]
namespace LocationSample.Droid
{
    public class LocationListener : AppCompatActivity, ILocationListener, ILocationHelper
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
        int messageId = 3;
        Android.App.NotificationManager manager;



        const long ONE_MINUTE = 60 * 1000;
        const long FIVE_MINUTES = 5 * ONE_MINUTE;
        static readonly string KEY_REQUESTING_LOCATION_UPDATES = "requesting_location_updates";
        protected LocationManager locationManager = (LocationManager)Android.App.Application.Context.GetSystemService(LocationService);
        static readonly int RC_LAST_LOCATION_PERMISSION_CHECK = 1000;
        static readonly int RC_LOCATION_UPDATES_PERMISSION_CHECK = 1100;
         bool isRequestingLocationUpdates;
        IFileLogger _logger;
        public event EventHandler<LocationData> OnLocationReceived;

        public LocationListener()
        {
            _logger = DependencyService.Get<IFileLogger>();
            isRequestingLocationUpdates = false;

        }


        public void OnLocationChanged(Location location)
        {
            _logger.LogInformation($"{DateTime.Now} - Lat: {location.Latitude} , Long: {location.Longitude} , Speed: {location.Speed * 3.6}, Time: {DateTime.Now}");
            OnLocationReceived.Invoke(this, new LocationData()
            {
                Latitude = location.Latitude,
                Longitude = location.Longitude,
                Speed = location.Speed
            });
            System.Diagnostics.Debug.WriteLine(location.Latitude);
            System.Diagnostics.Debug.WriteLine(location.Longitude);
            System.Diagnostics.Debug.WriteLine(location.Speed);
            ScheduleNotification("Running", $"{DateTime.Now} - Lat: {location.Latitude} , Long: {location.Longitude} , Speed: {location.Speed * 3.6}, Time: {DateTime.Now}");
        }
        public void ScheduleNotification(string title, string message)
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
                .SetPriority((int)NotificationPriority.Low)
                .SetOngoing(true);

            var notification = builder.Build();
            manager.Notify(messageId, notification);

            
        }

        void CreateNotificationChannel()
        {
            manager = (Android.App.NotificationManager)AndroidApp.Context.GetSystemService(AndroidApp.NotificationService);

            if (Build.VERSION.SdkInt >= BuildVersionCodes.O)
            {
                var channelNameJava = new Java.Lang.String(channelName);
                var channel = new Android.App.NotificationChannel(channelId, channelNameJava, Android.App.NotificationImportance.Low)
                {
                    Description = channelDescription
                };
                manager.CreateNotificationChannel(channel);
            }

            channelInitialized = true;
        }
        public void OnProviderDisabled(string provider)
        {
            isRequestingLocationUpdates = false;
        }

        public void OnProviderEnabled(string provider)
        {
            // Nothing to do in this example.
            Log.Debug("LocationExample", "The provider " + provider + " is enabled.");
        }
 
        private void FetchRequestingLocationUpdates()
        {

             locationManager.RequestLocationUpdates(LocationManager.GpsProvider, 1000*3, 0, this);
             locationManager.RequestLocationUpdates(LocationManager.NetworkProvider, 1000*3, 0, this);
        }

        void StopRequestingLocationUpdates()
        {
            locationManager.RemoveUpdates(this);
        }

        public void OnStatusChanged(string provider, [GeneratedEnum] Availability status, Bundle extras)
        {
            if (status == Availability.OutOfService)
            {
                StopRequestingLocationUpdates();
                isRequestingLocationUpdates = false;
            }
        }

        public void Start()
        {
            FetchRequestingLocationUpdates();
            isRequestingLocationUpdates = true;
        }

        public void Stop()
        {
        }

        public void PositionChanged()
        {
        }


        //void RequestLocationUpdatesButtonOnClick(object sender, EventArgs eventArgs)
        //{
        //    if (isRequestingLocationUpdates)
        //    {
        //        isRequestingLocationUpdates = false;
        //        StopRequestingLocationUpdates();
        //    }
        //    else
        //    {
        //        if (ContextCompat.CheckSelfPermission(this, Manifest.Permission.AccessFineLocation) == Permission.Granted)
        //        {
        //            StartRequestingLocationUpdates();
        //            isRequestingLocationUpdates = true;
        //        }
        //        else
        //        {
        //            RequestLocationPermission(RC_LAST_LOCATION_PERMISSION_CHECK);
        //        }
        //    }
        //}

        //void GetLastLocationButtonOnClick(object sender, EventArgs eventArgs)
        //{
        //    if (ContextCompat.CheckSelfPermission(this, Manifest.Permission.AccessFineLocation) == Permission.Granted)
        //    {
        //        GetLastLocationFromDevice();
        //    }
        //    else
        //    {
        //        RequestLocationPermission(RC_LAST_LOCATION_PERMISSION_CHECK);
        //    }
        //}

        //void GetLastLocationFromDevice()
        //{
        //    getLastLocationButton.SetText(Resource.String.getting_last_location);

        //    var criteria = new Criteria { PowerRequirement = Power.Medium };

        //    var bestProvider = locationManager.GetBestProvider(criteria, true);
        //    var location = locationManager.GetLastKnownLocation(bestProvider);

        //    if (location != null)
        //    {
        //        latitude.Text = Resources.GetString(Resource.String.latitude_string, location.Latitude);
        //        longitude.Text = Resources.GetString(Resource.String.longitude_string, location.Longitude);
        //        provider.Text = Resources.GetString(Resource.String.provider_string, location.Provider);
        //        getLastLocationButton.SetText(Resource.String.get_last_location_button_text);
        //    }
        //    else
        //    {
        //        latitude.SetText(Resource.String.location_unavailable);
        //        longitude.SetText(Resource.String.location_unavailable);
        //        provider.Text = Resources.GetString(Resource.String.provider_string, bestProvider);
        //        getLastLocationButton.SetText(Resource.String.get_last_location_button_text);
        //    }
        //}


    }

}
