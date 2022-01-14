using Android.Bluetooth;
using IncliForms.Controls;
using IncliForms.Utility;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Timers;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using IncliForms.Models.Inclinometer;
using System.Collections.Specialized;
using IncliForms.Services;
using System.Threading.Tasks;

namespace IncliForms.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AdrHarvest : ContentPage
    {
        private readonly DeviceBlock deviceBlock;
        private Models.Settings settings;
        private Timer timerReadData;
        private float A = 0;
        private float B = 0;
        private float Depth = 0;
        private readonly ObservableCollection<AdrDatablock> datalist = new ObservableCollection<AdrDatablock>();
        // 
        private readonly bool ShouldSwitchAandBValues = false;

        public AdrHarvest()
        {
            InitializeComponent();
        }

        public AdrHarvest(DeviceBlock deviceBlock)
        {
            InitializeComponent();
            this.deviceBlock = deviceBlock;
            Depth = deviceBlock.Inclinometer.StartDepth + .5f;
            lblDepth.Text = $"Dp: {Depth - .5f:000.0}";

            new System.Threading.Thread(async () =>
            {
                while (Navigation.NavigationStack.Count == 0)
                {
                    System.Threading.Thread.Sleep(50);
                }

                MainThread.BeginInvokeOnMainThread(() =>
                {
                    for (int i = 1; i <= Navigation.NavigationStack.Count - 2; i++)
                    {
                        Navigation.RemovePage(Navigation.NavigationStack[i]);
                    }
                });

                SettingsAccess acc = new SettingsAccess();
                settings = await acc.GetItemAsync();

                switch (settings.RecordUnit)
                {
                    case Models.RecordUnit.cm:
                        lblAUnit.Text = "cm";
                        lblBUnit.Text = "cm";
                        break;
                    case Models.RecordUnit.m:
                        lblAUnit.Text = "m";
                        lblBUnit.Text = "m";
                        break;
                    case Models.RecordUnit.mm:
                        lblAUnit.Text = "mm";
                        lblBUnit.Text = "mm";
                        break;
                }

                timerReadData = new Timer(settings.RefreshInterval);
                timerReadData.Elapsed += TimerReadData_Elapsed;
                timerReadData.AutoReset = true;
                timerReadData.Enabled = true;

            }).Start();

            listViewMain.ItemsSource = datalist;

            for (float i = deviceBlock.Inclinometer.StartDepth; i >= deviceBlock.Inclinometer.EndDepth; i -= 0.5f)
            {
                var data = new AdrDatablock
                {
                    Aplus = 0,
                    Bplus = 0,
                    Depth = i,
                    Aminus = 0,
                    Bminus = 0
                };
                datalist.Add(data);
            }

            listViewMain.SelectedItem = datalist[0] ?? null;
        }

        private async void TimerReadData_Elapsed(object sender, ElapsedEventArgs ev)
        {
            try
            {
                string res = (await App.Bluetooth.ReadData());
                if (res == null) return;

                //if (res.Any("!@#$%^&*()_+GHIJKLMNOPQRSTUVWXYZ".Contains)) return;

                int a = res.IndexOf('A');
                int b = res.IndexOf('B');
                int c = res.IndexOf('C');
                int d = res.IndexOf('D');
                int e = res.IndexOf('E');
                int f = res.IndexOf('F');

                //if (a == -1 || b == -1 || c == -1 || d == -1 || e == -1 || f == - 1) return;
                if (a == -1 || b == -1 || c == -1 || d == -1 || e == -1) return;
                if (f == -1) f = res.Length - 1;

                // A
                string alpha = res.Substring(a + 1, (b - a) - 1);
                // B
                string beta = res.Substring(b + 1, (c - b) - 1);

                if (ShouldSwitchAandBValues)
                {
                    // Swap vars
                    string tempAlpha = alpha;
                    alpha = beta;
                    beta = tempAlpha;
                }

                // *
                string gamma = res.Substring(c + 1, (d - c) - 1);
                // Battery on board
                string delta = res.Substring(d + 1, (e - d) - 1);
                // Battery
                string epsilon = res.Substring(e + 1, (f - e) - 1);
                // *
                string zeta = res.Substring(f + 1);

                MainThread.BeginInvokeOnMainThread(() =>
                {
                    float.TryParse(alpha, out A);
                    float.TryParse(beta, out B);
                    //float.TryParse(delta, out float battery);
                    float.TryParse(epsilon, out float battery);

                    if (battery != 0)
                        battery = (battery * settings.BatteryMultiplier) + settings.BatteryOffset;

                    if (settings == null) return;

                    A = (A * settings.RotationalCorrA) + settings.SensitivityFactorA;
                    B = (B * settings.RotationalCorrB) + settings.SensitivityFactorB;

                    switch (settings.RecordUnit)
                    {
                        case Models.RecordUnit.m:
                            A = A / 100;
                            B = B / 100;
                            lblA.Text = $"A: {A:#####.#####}";
                            lblB.Text = $"B: {B:#####.#####}";
                            break;
                        case Models.RecordUnit.cm:
                            lblA.Text = $"A: {A:#####.###}";
                            lblB.Text = $"B: {B:#####.###}";
                            break;
                        case Models.RecordUnit.mm:
                            A = A * 10;
                            B = B * 10;
                            lblA.Text = $"A: {A:#####.##}";
                            lblB.Text = $"B: {B:#####.##}";
                            break;
                    }

                    #region Battery

                    // normalize to 0 < x < 1
                    float normalizedProgress = (battery - 6.5f) / (8.4f - 6.5f);

                    // discretion
                    if (normalizedProgress >= .75f) // more than 75
                    {
                        normalizedProgress = 1f;
                        progressBattery.ProgressColor = Color.Green;
                    }
                    if (normalizedProgress >= .5f && normalizedProgress < .75f) // between 75 and 50
                    {
                        normalizedProgress = 0.75f;
                        progressBattery.ProgressColor = Color.Green;
                    }
                    else if (normalizedProgress >= 0.25 && normalizedProgress < 0.5f) // between 50 and 25
                    {
                        normalizedProgress = 0.5f;
                        progressBattery.ProgressColor = Color.Yellow;
                    }
                    else if (normalizedProgress <= 0.25f) // less than 25
                    {
                        normalizedProgress = 0.25f;
                        progressBattery.ProgressColor = Color.Red;
                    }

                    progressBattery.Progress = normalizedProgress;

                    lblBatteryPercentage.Text = $"{Math.Round(progressBattery.Progress * 100)}%";

                    #endregion

                });
            }
            catch (Exception ex)
            {
                if (ex.Message == "Socket Closed")
                {
                    timerReadData.Enabled = false;
                    return;
                }
            }
        }

        protected override bool OnBackButtonPressed()
        {
            ContentPage_Disappearing(this, null);

            return base.OnBackButtonPressed();
        }

        private void ContentPage_Disappearing(object sender, EventArgs e)
        {
            for (int i = 1; i <= Navigation.NavigationStack.Count - 2; i++)
            {
                Navigation.RemovePage(Navigation.NavigationStack[i]);
            }

            timerReadData.Enabled = false;
            App.Bluetooth.Disconnect();
        }

        bool isSecondPhase = false;
        private void LogRecord_Tapped(object sender, EventArgs e)
        {
            AdrDatablock block = (AdrDatablock)listViewMain.SelectedItem;
            int dataIndex = datalist.IndexOf(block);

            //scrollViewMain.ScrollToAsync(0, scrollViewMain.ScrollY + 48, true);
            listViewMain.ScrollTo(listViewMain.SelectedItem, ScrollToPosition.Center, false);

            btnLog.BackgroundColor = Color.GreenYellow;

            var a = new Animation((v) => btnLog.BackgroundColor =
            Color.FromHsla(v + 0.10, Color.Orange.Saturation, Color.Orange.Luminosity),
            Color.Orange.Luminosity, Color.Orange.Hue - 0.10f);
            a.Commit(this, "hello", rate: 16, length: 1200);

            if (!isSecondPhase)
            {
                // First Phase
                block.Aplus = A;
                block.Bplus = B;
            }
            else
            {
                // Second Phase
                block.Aminus = A;
                block.Bminus = B;
                block.CalculateDeltas();
            }

            datalist[dataIndex] = block;

            //- Second Phase Started
            if (!isSecondPhase && (dataIndex + 1) >= datalist.Count)
            {
                EndHarvest_Tapped(this, e);
                return;
            }

            //- End
            if (isSecondPhase && (dataIndex + 1) >= datalist.Count)
            {
                EndHarvest_Tapped(this, e);
                listViewMain.SelectedItem = null;
                return;
            }

            // Label
            Depth -= 0.5f;
            lblDepth.Text = $"Dp: {Depth - .5f:000.0}";

            App.PlaySound("Soundtracks/Recorded.mp3");


            // Select the next block
            listViewMain.SelectedItem = datalist[dataIndex + 1];
        }

        private void SecondPhaseStarted()
        {
            Depth = deviceBlock.Inclinometer.StartDepth + .5f;
            lblDepth.Text = $"Dp: {Depth - .5f:000.0}";
            isSecondPhase = true;
            App.PlaySound("Soundtracks/SecondPhase.mp3");
            if (!switchAuto.IsToggled)
                DisplayAlert("Second phase started", "Second Phase has started, Turn the probe 180 degrees", "Ok");
            listViewMain.SelectedItem = datalist[0];
            listViewMain.ScrollTo(listViewMain.SelectedItem, ScrollToPosition.MakeVisible, false);

            if (switchAuto.IsToggled)
            {
                // Copy all data and end
                foreach (AdrDatablock datablock in datalist)
                {
                    datablock.Aminus = datablock.Aplus * -1;
                    datablock.Bminus = datablock.Bplus * -1;
                    datablock.CalculateDeltas();
                }

                // End harvest
                EndHarvest_Tapped(this, null);

            }


        }

        private void EndHarvest_Tapped(object sender, EventArgs e)
        {
            // Check if all blocks have value
            if (!isSecondPhase)
            {
                // First Phase

                var unentered = datalist.ToList().Where(i => (i.Aplus == 0) || (i.Bplus == 0));

                if (unentered.Any())
                {
                    App.ToastShort("Not all depth levels have a value");
                    return;
                }

                SecondPhaseStarted();
            }
            else if (isSecondPhase)
            {
                // Second Phase

                var unentered = datalist.ToList().Where(i => (i.Aplus == 0) || (i.Aminus == 0) || (i.Bplus == 0) || (i.Bminus == 0));

                if (unentered.Any())
                {
                    App.ToastShort("Not all depth levels have a value");
                    return;
                }

                btnLog.IsEnabled = false;
                btnLog.GestureRecognizers.Clear();

                Navigation.PushAsync(new AdrDataHarvestCompleted(
                    deviceBlock.Inclinometer,
                    datalist.Reverse())
                    );
            }
        }

        private void UpClicked_Tapped(object sender, EventArgs e)
        {
            int selectedIndex = datalist.IndexOf((AdrDatablock)listViewMain.SelectedItem);
            if (selectedIndex - 1 < 0) return;
            listViewMain.SelectedItem = datalist[selectedIndex - 1];
        }

        private void DownClicked_Tapped(object sender, EventArgs e)
        {
            int selectedIndex = datalist.IndexOf((AdrDatablock)listViewMain.SelectedItem);
            if (selectedIndex + 1 >= datalist.Count) return;
            listViewMain.SelectedItem = datalist[selectedIndex + 1];
        }

        private void ListViewMain_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (!(listViewMain.SelectedItem is AdrDatablock selected)) return;
            lblDepth.Text = $"Dp: {selected.Depth:000.0}";

            listViewMain.ScrollTo(
                selected,
                ScrollToPosition.MakeVisible,
                true);
        }
    }
}