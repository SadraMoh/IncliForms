using IncliForms.Utility;
using IncliForms.Views;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace IncliForms
{
    public partial class App : Application
    {
        public static bool IsAdmin { get; set; } = false;
        public readonly static string Password = "Azda";
        public static void ToastShort(string message)
        {
            DependencyService.Get<IToast>().ShortAlert(message);
        }
        public static void ToastLong(string message)
        {
            DependencyService.Get<IToast>().LongAlert(message);
        }

        public static void PlaySound(string filename)
        {
            DependencyService.Get<IAudio>().PlayAudioFile(filename);
        }

        public static IBluetoothSerial Bluetooth { get; set; }
        public static void Init(IBluetoothSerial bluetoothSerial)
        {
            App.Bluetooth = bluetoothSerial;
        }

        public App()
        {
            InitializeComponent();

            //_bluetoothAdapter = DependencyService.Get<IAdapter>();

            //_bluetoothAdapter.ScanTimeout = TimeSpan.FromSeconds(12);
            //_bluetoothAdapter.ConnectionTimeout = TimeSpan.FromSeconds(12);

            MainPage = new AppShell();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
