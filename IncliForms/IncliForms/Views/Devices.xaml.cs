using IncliForms.Controls;
using IncliForms.Models;
using IncliForms.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using IncliForms.Models.Inclinometer;

namespace IncliForms.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Devices : ContentPage
    {
        public AdrInclinometerAccess access;

        public Devices()
        {
            InitializeComponent();
            access = new AdrInclinometerAccess();
        }

        private void ContentPage_Appearing(object sender, EventArgs e)
        {
            RefreshBlocks(sender, e);
            Navigation.NavigationStack.GetEnumerator().Dispose();
        }

        int gridCounter = 0;
        void addDeviceToGrid(AdrInclinometer model)
        {
            DeviceBlock device = new DeviceBlock(model);
            device.Clicked += DeviceBlock_Clicked;

            if (gridCounter % 2 == 0)
                stackFirst.Children.Add(device);
            else
                stackSecond.Children.Add(device);
            ++gridCounter;
        }

        public async void RefreshBlocks(object sender, EventArgs e)
        {
            stackFirst.Children.Clear();
            stackSecond.Children.Clear();

            var devices = await access.GetItemsAsync();

            gridCounter = 0;
            foreach (var device in devices)
                addDeviceToGrid(device);

            AddDeviceButton addDeviceButton = new AddDeviceButton();
            addDeviceButton.Clicked += AddDevice_Clicked;

            if (gridCounter % 2 == 0)
                stackFirst.Children.Add(addDeviceButton);
            else
                stackSecond.Children.Add(addDeviceButton);
        }

        private void AddDevice_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new AddDevice(this));
        }

        //[Obsolete]
        //private async void Connect(object sender, EventArgs e)
        //{

        //    while (true)
        //    {
        //        string a = await App.Bluetooth.ReadData();
        //        await DisplayAlert("", a, "Ok");
        //    }

        //    //App.BluetoothAdapter.DeviceDisconnected += (a, b) =>
        //    //{
        //    //    DisplayAlert("", "DeviceDisconnected", "Ok");
        //    //};
        //    //App.BluetoothAdapter.DeviceFailedToConnect += (a, b) =>
        //    //{
        //    //    DisplayAlert("", "DeviceFailedToConnect", "Ok");
        //    //};
        //    //App.BluetoothAdapter.ScanTimeoutElapsed += (a, b) =>
        //    //{
        //    //    DisplayAlert("", "ScanTimeoutElapsed", "Ok");
        //    //};
        //    //App.BluetoothAdapter.DeviceDiscovered += (a, b) =>
        //    //{
        //    //    IDevice device = (IDevice)a;
        //    //    App.BluetoothAdapter.ConnectToDevice(device);
        //    //};

        //    //App.BluetoothAdapter.StartScanningForDevices();

        //    //App.BluetoothAdapter.DiscoveredDevices.ToList().ForEach(i =>
        //    //{
        //    //    DisplayAlert("Device", $"{i.Name} {i.NativeDevice} {i.Rssi} {i.State}", "Ok");
        //    //    i.Services.ToList().ForEach(s =>
        //    //    {
        //    //        DisplayAlert("Services", $"{s.Id} {s.IsPrimary} {s.Uuid}", "Ok");
        //    //    });
        //    //});

        //}

        private async void DeviceBlock_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Connecting(this, (DeviceBlock)sender));
        }

    }
}