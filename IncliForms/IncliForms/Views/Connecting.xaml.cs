using IncliForms.Controls;
using IncliForms.Utility;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace IncliForms.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Connecting : ContentPage
    {
        public Devices DevicesPage;
        private readonly DeviceBlock DeviceBlock;

        private class Vm : INotifyPropertyChanged
        {
            public event EventHandler BackButtonPressed;
            public event PropertyChangedEventHandler PropertyChanged;

            public ICommand cmdClick { get; set; }

            public Vm()
            {
                cmdClick = new Command(
               execute: () => { BackButtonPressed.Invoke(this, null); }
               , canExecute: () => true
               );
            }
        }


        public Connecting()
        {
            InitializeComponent();
        }

        readonly System.Timers.Timer Timer = new System.Timers.Timer(12000);
        public Connecting(Devices devicesPage, DeviceBlock deviceBlock)
        {
            InitializeComponent();
            this.DevicesPage = devicesPage;
            this.DeviceBlock = deviceBlock;

            ConnectToDevice();

            Vm vm = new Vm();
            vm.BackButtonPressed += btnCancel_Clicked;
            this.BindingContext = vm;
        }

        public async void ConnectToDevice()
        {
            try
            {
                Timer.Elapsed += (sender, e) =>
                {
                    throw new Exception("Connection Timed out");
                };
                Timer.Enabled = true;
                var bluetoothDevice = await App.Bluetooth.ConnectToDevice(DeviceBlock.Inclinometer.BluetoothUUID);
                Timer.Enabled = false;
                await Navigation.PushAsync(new AdrHarvest(DeviceBlock));
            }
            catch (Exception ex)
            {
                DependencyService.Get<IToast>().ShortAlert(ex.Message);
                Timer.Enabled = false;
                await Navigation.PushAsync(new ConnectionFailed(this));
            }
        }

        private async void btnCancel_Clicked(object sender, EventArgs e)
        {
            Timer.Enabled = false;

            if (!(App.Bluetooth.Device is null) || App.Bluetooth.Device != null)
                App.Bluetooth.Disconnect();

            await Navigation.PopAsync();
        }
    }
}