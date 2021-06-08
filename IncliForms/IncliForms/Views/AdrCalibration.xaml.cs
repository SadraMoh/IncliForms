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
    public partial class AdrCalibration : ContentPage
    {
        public AdrInclinometer Inclinometer { get; }
        AdrInclinometerAccess Access = new AdrInclinometerAccess();

        public AdrCalibration()
        {
            InitializeComponent();
        }

        public AdrCalibration(AdrInclinometer inclinometer)
        {
            InitializeComponent();
            Inclinometer = inclinometer;

            this.Title = Inclinometer.BoreholeName + " Calibration";

            lblRotA.Text = inclinometer.RotationalCorrA.ToString();
            lblRotB.Text = inclinometer.RotationalCorrB.ToString();
            lblSensA.Text = inclinometer.SensitivityFactorA.ToString();
            lblSensB.Text = inclinometer.SensitivityFactorB.ToString();
        }

        private async void RotA_Tapped(object sender, EventArgs e)
        {
        askAgain:
            string res = await DisplayPromptAsync("Rotational Corr A", null, placeholder: lblRotA.Text);
            if (string.IsNullOrEmpty(res)) return;
            if (!int.TryParse(res, out int pres))
            {
                App.ToastShort("Invalid Value");
                goto askAgain;
            }

            Inclinometer.RotationalCorrA = pres;
            lblRotA.Text = res;
            await Access.SaveItemAsync(Inclinometer);
        }

        private async void RotB_Tapped(object sender, EventArgs e)
        {
        askAgain:
            string res = await DisplayPromptAsync("Rotational Corr B", null, placeholder: lblRotB.Text);
            if (string.IsNullOrEmpty(res)) return;
            if (!int.TryParse(res, out int pres))
            {
                App.ToastShort("Invalid Value");
                goto askAgain;
            }

            Inclinometer.RotationalCorrB = pres;
            lblRotB.Text = res;
            await Access.SaveItemAsync(Inclinometer);
        }

        private async void SensA_Tapped(object sender, EventArgs e)
        {
        askAgain:
            string res = await DisplayPromptAsync("Sensitivity Factor A", null, placeholder: lblSensA.Text);
            if (string.IsNullOrEmpty(res)) return;
            if (!int.TryParse(res, out int pres))
            {
                App.ToastShort("Invalid Value");
                goto askAgain;
            }

            Inclinometer.SensitivityFactorA = pres;
            lblSensA.Text = res;
            await Access.SaveItemAsync(Inclinometer);
        }

        private async void SensB_Tapped(object sender, EventArgs e)
        {
        askAgain:
            string res = await DisplayPromptAsync("Sensitivity Factor B", null, placeholder: lblSensB.Text);
            if (string.IsNullOrEmpty(res)) return;
            if (!int.TryParse(res, out int pres))
            {
                App.ToastShort("Invalid Value");
                goto askAgain;
            }

            Inclinometer.SensitivityFactorB = pres;
            lblSensB.Text = res;
            await Access.SaveItemAsync(Inclinometer);
        }
    }
}