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
    public partial class AdrSettings : ContentPage
    {
        public AdrInclinometer Inclinometer { get; }
        AdrInclinometerAccess Access = new AdrInclinometerAccess();

        public AdrSettings()
        {
            InitializeComponent();
        }

        public AdrSettings(AdrInclinometer inclinometer)
        {
            InitializeComponent();
            this.Inclinometer = inclinometer;

            this.Title = Inclinometer.BoreholeName + " Settings";

            lblBoreholeName.Text = Inclinometer.BoreholeName;
            lblModelName.Text = Inclinometer.ModelName;
            lblVersion.Text = Inclinometer.VersionName;
            lblUUID.Text = Inclinometer.BluetoothUUID;
            lblBoreholeDepth.Text = Inclinometer.BoreholeDepth.ToString();
            lblBoreholeNumber.Text = Inclinometer.BoreholeNumber.ToString();
            lblStartDepth.Text = Inclinometer.StartDepth.ToString();
            lblEndDepth.Text = Inclinometer.EndDepth.ToString();
            lblProbeSerial.Text = Inclinometer.ProbeSerial.ToString();
            lblReelSerial.Text = Inclinometer.ReelSerial.ToString();
        }

        private async void cellCalibtration_Tapped(object sender, EventArgs e)
        {
            if (App.IsAdmin)
                await Navigation.PushAsync(new AdrCalibration(Inclinometer));
            else
                App.ToastShort("Admin Privilages Required");
        }

        private async void DeleteDevice_Clicked(object sender, EventArgs e)
        {
            if (!await DisplayAlert("Confirm Delete", "Are you sure you want to remove this device?", "Delete", "Cancel")) return;
            Access.DeleteItemById(Inclinometer.Id);
            await Navigation.PopAsync();
        }

        private async void cellBoreholeName_Tapped(object sender, EventArgs e)
        {
            string res = await DisplayPromptAsync("Borehole Name", null, placeholder: lblBoreholeName.Text);
            if (string.IsNullOrEmpty(res)) return;

            Inclinometer.BoreholeName = res;
            lblBoreholeName.Text = res;
            await Access.SaveItemAsync(Inclinometer);
        }

        private async void cellBoreholeNumber_Tapped(object sender, EventArgs e)
        {
            string res = await DisplayPromptAsync("Borehole Number", null, placeholder: lblBoreholeNumber.Text);
            if (string.IsNullOrEmpty(res)) return;

            Inclinometer.BoreholeNumber = res;
            lblBoreholeNumber.Text = res;
            await Access.SaveItemAsync(Inclinometer);
        }

        private async void cellBoreholeDepth_Tapped(object sender, EventArgs e)
        {
        askAgain:
            string res = await DisplayPromptAsync("Borehole Depth", null, placeholder: lblBoreholeDepth.Text);
            if (string.IsNullOrEmpty(res)) return;
            if (!float.TryParse(res, out float pres))
            {
                App.ToastShort("Invalid Value");
                goto askAgain;
            }

            Inclinometer.BoreholeDepth = pres;
            lblBoreholeDepth.Text = res;
            await Access.SaveItemAsync(Inclinometer);
        }

        private async void cellStartDepth_Tapped(object sender, EventArgs e)
        {
        askAgain:
            string res = await DisplayPromptAsync("Reading Start Depth", null, placeholder: lblStartDepth.Text);
            if (string.IsNullOrEmpty(res)) return;
            if (!float.TryParse(res, out float pres))
            {
                App.ToastShort("Invalid Value");
                goto askAgain;
            }
            if (pres > Inclinometer.BoreholeDepth)
            {
                App.ToastShort($"Start Depth can't be greater than Borehole Depth ({Inclinometer.BoreholeDepth})");
                goto askAgain;
            }
            if (pres <= Inclinometer.EndDepth)
            {
                App.ToastShort($"Start Depth can't be less than End Depth ({Inclinometer.EndDepth})");
                goto askAgain;
            }
            if (pres <= 0)
            {
                App.ToastShort($"Start Depth can't be less than Zero");
                goto askAgain;
            }


            Inclinometer.StartDepth = pres;
            lblStartDepth.Text = res;
            await Access.SaveItemAsync(Inclinometer);
        }

        private async void cellEndDepth_Tapped(object sender, EventArgs e)
        {
        askAgain:
            string res = await DisplayPromptAsync("Reading End Depth", null, placeholder: lblEndDepth.Text);
            if (string.IsNullOrEmpty(res)) return;
            if (!float.TryParse(res, out float pres))
            {
                App.ToastShort("Invalid Value");
                goto askAgain;
            }
            if(pres > Inclinometer.BoreholeDepth)
            {
                App.ToastShort($"End Depth can't be greater than Borehole Depth ({Inclinometer.BoreholeDepth})");
                goto askAgain;
            }
            if(pres > Inclinometer.StartDepth)
            {
                App.ToastShort($"End Depth can't be greater than Start Depth ({Inclinometer.StartDepth})");
                goto askAgain;
            }
            if(pres < 0)
            {
                App.ToastShort($"End Depth can't be less than Zero");
                goto askAgain;
            }


            Inclinometer.EndDepth = pres;
            lblEndDepth.Text = res;
            await Access.SaveItemAsync(Inclinometer);
        }
    }
}