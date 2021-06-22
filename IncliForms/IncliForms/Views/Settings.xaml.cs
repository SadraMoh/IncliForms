using IncliForms.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace IncliForms.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Settings : ContentPage
    {
        SettingsAccess Access = new SettingsAccess();
        Models.Settings Def;

        public Settings()
        {
            InitializeComponent();
            Init();
        }

        private void Init()
        {
            Task.Run(async () =>
            {
                Def = (await Access.GetItemAsync());

                MainThread.BeginInvokeOnMainThread(() =>
                {
                    lblOperatorName.Text = Def.OperatorName;
                    lblWorkplaceName.Text = Def.SiteName;
                    lblRefreshInterval.Text = Def.RefreshInterval.ToString() + "ms";
                    lblBatteryMultiplier.Text = Def.BatteryMultiplier.ToString();
                    lblBatteryOffset.Text = Def.BatteryOffset.ToString();
                    lblRotationalCorrA.Text = Def.RotationalCorrA.ToString();
                    lblRotationalCorrB.Text = Def.RotationalCorrB.ToString();
                    lblSensitivityFactorA.Text = Def.SensitivityFactorA.ToString();
                    lblSensitivityFactorB.Text = Def.SensitivityFactorB.ToString();
                    pickUnit.SelectedIndex = (int)Def.RecordUnit;
                });

            });
        }

        private async void SwitchCell_OnChanged(object sender, ToggledEventArgs e)
        {

            var swCell = ((SwitchCell)sender);

            async Task<bool> check()
            {
                if (swCell.On)
                {
                    // Turn On Admin Mode
                    string res = await DisplayPromptAsync("Enter Password", "To unlock Admin Mode, enter the password");
                    if (string.IsNullOrEmpty(res)) return false;
                    if (res == App.Password)
                    {
                        App.IsAdmin = true;
                        App.ToastShort("Admin Mode On");
                        return true;
                    }
                }
                else
                {
                    // Turn Off Admin Mode
                    App.IsAdmin = false;
                    App.ToastShort("Admin Mode Off");
                    return false;
                }
                return false;
            }

            swCell.On = await check();

        }

        private async void ViewCell_Tapped(object sender, EventArgs e)
        {
            string res = await DisplayPromptAsync("Enter Workplace Name", null, placeholder: lblWorkplaceName.Text);
            if (string.IsNullOrEmpty(res)) return;

            Def.SiteName = res;
            await Access.SaveItemAsync(Def);
            lblWorkplaceName.Text = res;
        }

        private async void ViewCell_Tapped_1(object sender, EventArgs e)
        {
            string res = await DisplayPromptAsync("Enter Operator Name", null, placeholder: lblOperatorName.Text);
            if (string.IsNullOrEmpty(res)) return;

            Def.OperatorName = res;
            await Access.SaveItemAsync(Def);
            lblOperatorName.Text = res;
        }

        private async void Unit_Tapped(object sender, EventArgs e)
        {
            Def.RecordUnit = (Models.RecordUnit)pickUnit.SelectedIndex;
            await Access.SaveItemAsync(Def);
        }

        private async void RefreshInterval_Tapped(object sender, EventArgs e)
        {
            string res = await DisplayPromptAsync("Enter Refresh Interval in miliseconds", null, placeholder: lblRefreshInterval.Text);
            if (string.IsNullOrEmpty(res)) return;
            if (!int.TryParse(res, out int tres)) return;
            if (tres < 100)
            {
                App.ToastShort("Interval cannot be less than 100 miliseconds");
                return;
            };

            Def.RefreshInterval = tres;
            await Access.SaveItemAsync(Def);
            lblRefreshInterval.Text = res + "ms";
        }

        private async void RotationalCorrA_Tapped(object sender, EventArgs e)
        {
            if (!App.IsAdmin)
            {
                App.ToastShort("Admin Mode Required");
                return;
            }

            string res = await DisplayPromptAsync("Enter RotationalCorrA", null, placeholder: lblRotationalCorrA.Text);
            if (string.IsNullOrEmpty(res)) return;
            if (!float.TryParse(res, out float resf)) return;

            Def.RotationalCorrA = resf;
            await Access.SaveItemAsync(Def);
            lblRotationalCorrA.Text = res;
        }

        private async void RotationalCorrB_Tapped(object sender, EventArgs e)
        {
            if (!App.IsAdmin)
            {
                App.ToastShort("Admin Mode Required");
                return;
            }

            string res = await DisplayPromptAsync("Enter RotationalCorrB", null, placeholder: lblRotationalCorrB.Text);
            if (string.IsNullOrEmpty(res)) return;
            if (!float.TryParse(res, out float resf)) return;

            Def.RotationalCorrB = resf;
            await Access.SaveItemAsync(Def);
            lblRotationalCorrB.Text = res;
        }

        private async void SensitivityFactorA_Tapped(object sender, EventArgs e)
        {
            if (!App.IsAdmin)
            {
                App.ToastShort("Admin Mode Required");
                return;
            }

            string res = await DisplayPromptAsync("Enter  SensitivityFactorA", null, placeholder: lblSensitivityFactorA.Text);
            if (string.IsNullOrEmpty(res)) return;
            if (!float.TryParse(res, out float resf)) return;

            Def.SensitivityFactorA = resf;
            await Access.SaveItemAsync(Def);
            lblSensitivityFactorA.Text = res;
        }

        private async void SensitivityFactorB_Tapped(object sender, EventArgs e)
        {
            if (!App.IsAdmin)
            {
                App.ToastShort("Admin Mode Required");
                return;
            }


            string res = await DisplayPromptAsync("Enter SensitivityFactorB", null, placeholder: lblSensitivityFactorB.Text);
            if (string.IsNullOrEmpty(res)) return;
            if (!float.TryParse(res, out float resf)) return;

            Def.SensitivityFactorB = resf;
            await Access.SaveItemAsync(Def);
            lblSensitivityFactorB.Text = res;
        }

        private async void BatteryMultiplier_Tapped(object sender, EventArgs e)
        {
            if (!App.IsAdmin)
            {
                App.ToastShort("Admin Mode Required");
                return;
            }

            string res = await DisplayPromptAsync("Enter BatteryMultiplier", null, placeholder: lblBatteryMultiplier.Text);
            if (string.IsNullOrEmpty(res)) return;
            if (!float.TryParse(res, out float resf)) return;

            Def.BatteryMultiplier = resf;
            await Access.SaveItemAsync(Def);
            lblBatteryMultiplier.Text = res;
        }


        private async void BatteryOffset_Tapped(object sender, EventArgs e)
        {
            if (!App.IsAdmin)
            {
                App.ToastShort("Admin Mode Required");
                return;
            }

            string res = await DisplayPromptAsync("Enter BatteryOffset", null, placeholder: lblBatteryOffset.Text);
            if (string.IsNullOrEmpty(res)) return;
            if (!float.TryParse(res, out float resf)) return;

            Def.BatteryOffset = resf;
            await Access.SaveItemAsync(Def);
            lblBatteryOffset.Text = res;
        }

    }
}