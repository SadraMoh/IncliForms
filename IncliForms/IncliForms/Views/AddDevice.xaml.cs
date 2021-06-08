using IncliForms.Models;
using IncliForms.Models.Inclinometer;
using IncliForms.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace IncliForms.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddDevice : ContentPage
    {
        public Devices PageDevices { get; set; }
        public AddDevice(Devices devices)
        {
            InitializeComponent();
            this.PageDevices = devices;
            initFields();
        }

        private async void initFields()
        {
            SettingsAccess settingsAccess = new SettingsAccess();
            var settings = await settingsAccess.GetItemAsync();

            AdrInclinometerAccess inclinometerAccess = new AdrInclinometerAccess();

            var adrInclinometers = await inclinometerAccess.GetItemsAsync();
            txtBoreholeNo.Text = (adrInclinometers.Count + 1).ToString();

            var a = App.Bluetooth.GetNearbyDevices();
            var names = a.Select(i => i.Name);
            pickBluetooth.ItemsSource = names.ToList();
            pickBluetooth.SelectedIndex = 0;

            pickDeviceModel.SelectedIndex = 0;
        }

        private async void btnAdd_Clicked(object sender, EventArgs e)
        {
            AdrInclinometerAccess inclinometerAccess = new AdrInclinometerAccess();

            #region Checks
            if (string.IsNullOrEmpty(txtBoreholeNo.Text))
            {
                await DisplayAlert("", "Please Enter Borehole Number", "Ok");
                return;
            }
            if (string.IsNullOrEmpty(txtDepth.Text))
            {
                await DisplayAlert("", "Please Enter Depth", "Ok");
                return;
            }
            if (string.IsNullOrEmpty(txtProbeSerial.Text))
            {
                await DisplayAlert("", "Please Enter Probe Serial", "Ok");
                return;
            }
            if (string.IsNullOrEmpty(txtReelSerial.Text))
            {
                await DisplayAlert("", "Please Enter Reel Serial", "Ok");
                return;
            }
            if (string.IsNullOrEmpty(txtSiteName.Text))
            {
                await DisplayAlert("", "Please Enter Site Name", "Ok");
                return;
            }

            if (!float.TryParse(txtDepth.Text,out _))
            {
                await DisplayAlert("", "Depth Should be a number", "Ok");
                return;
            }
            if (!int.TryParse(txtProbeSerial.Text, out _))
            {
                await DisplayAlert("", "ProbeSerial Should be a number", "Ok");
                return;
            }
            if (!int.TryParse(txtReelSerial.Text, out _))
            {
                await DisplayAlert("", "ProbeSerial Should be a number", "Ok");
                return;
            }
            #endregion

            AdrInclinometer data = new AdrInclinometer()
            {
                BluetoothUUID = (string)pickBluetooth.SelectedItem,
                BoreholeName = txtSiteName.Text,
                BoreholeNumber = txtBoreholeNo.Text,
                BoreholeDepth = float.Parse(txtDepth.Text),
                ProbeSerial = int.Parse(txtProbeSerial.Text),
                ReelSerial = int.Parse(txtReelSerial.Text),
                StartDepth = float.Parse(txtDepth.Text),
                EndDepth = 0.5f
            };

            await inclinometerAccess.SaveItemAsync(data);

            await Navigation.PopAsync();
        }

        private async void btnCancel_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }
    }
}