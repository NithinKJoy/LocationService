using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Support.V4.App;
using Android;
using Android.Locations;
using Android.Content;
using Android.Views.Animations;
using Plugin.CurrentActivity;

namespace LocationSample.Droid
{
    [Activity(Label = "LocationSample", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        bool isRequestingLocationUpdates;
        static readonly string KEY_REQUESTING_LOCATION_UPDATES = "requesting_location_updates";
        LocationManager locationManager;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(savedInstanceState);
            locationManager = GetSystemService(LocationService) as LocationManager;

            if (ActivityCompat.ShouldShowRequestPermissionRationale(this, Manifest.Permission.AccessFineLocation))
            {
                ActivityCompat.RequestPermissions(this, new[] { Manifest.Permission.AccessFineLocation, Manifest.Permission.WriteExternalStorage }, 1);
            }
            else
            {
                ActivityCompat.RequestPermissions(this, new[] { Manifest.Permission.AccessFineLocation, Manifest.Permission.WriteExternalStorage }, 1);
            }
            CrossCurrentActivity.Current.Init(this, savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            LoadApplication(new App());

            if (savedInstanceState != null)
            {
                isRequestingLocationUpdates = savedInstanceState.KeySet().Contains(KEY_REQUESTING_LOCATION_UPDATES) &&
                                              savedInstanceState.GetBoolean(KEY_REQUESTING_LOCATION_UPDATES);
            }
            else
            {
                isRequestingLocationUpdates = false;
            }
       
        }
        protected override void OnDestroy()
        {
            base.OnDestroy();
            Xamarin.Forms.DependencyService.Get<IForegroundManager>().Stop();
        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}