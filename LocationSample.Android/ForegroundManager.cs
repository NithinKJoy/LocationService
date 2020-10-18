using System;
using Android.Content;
using LocationSample.Droid;
using Xamarin.Forms;

[assembly: Dependency(typeof(ForegroundManager))]
namespace LocationSample.Droid
{
    public class ForegroundManager : IForegroundManager
    {
        bool _hasStarted;

        Intent startServiceIntent, stopServiceIntent;
        public ForegroundManager()
        {

        }

        public void Start()
        {
            if (!_hasStarted)
            {
                var activity = (MainActivity)Forms.Context;
                startServiceIntent = new Intent(activity, typeof(ForegroundService));
                startServiceIntent.SetAction("LocationSample.Droid.action.START_SERVICE");

                if (Android.OS.Build.VERSION.SdkInt >= Android.OS.BuildVersionCodes.O)
                {
                    activity.StartForegroundService(startServiceIntent);
                }
                else
                {
                    activity.StartService(startServiceIntent);
                }

                _hasStarted = true;
            }
        }

        public void Stop()
        {
            if (_hasStarted)
            {
                var activity = (MainActivity)Forms.Context;
                stopServiceIntent = new Intent(activity, typeof(ForegroundService));
                stopServiceIntent.SetAction("LocationSample.Droid.action.STOP_SERVICE");
                activity.StopService(stopServiceIntent);
            }
        }
    }
}
