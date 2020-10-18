using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace LocationSample
{
    public partial class MainPage : ContentPage
    {
        protected IForegroundManager _foregroundManager;
        ILocationHelper _locationHelper; 

        public MainPage()
        {
            InitializeComponent();
            _foregroundManager = DependencyService.Get<IForegroundManager>();
            _locationHelper = DependencyService.Get<ILocationHelper>();
        }

        private void OnLocationFetched(object sender, LocationData e)
        {
            Btn_fetch.Text = "Receiving...";
            Lbl_lat.Text = e.Latitude.ToString();
            Lbl_long.Text = e.Longitude.ToString();
            Lbl_speed.Text = e.Speed.ToString();
        }

        void Button_Clicked(System.Object sender, System.EventArgs e)
        {
            Btn_fetch.Text = "Started Fetching";
            if (_locationHelper != null)
            {
                _locationHelper.OnLocationReceived += OnLocationFetched;
            }
            _foregroundManager.Start(); 
        }

       
    }
}
