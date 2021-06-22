using Android;
using IncliForms.Models.Inclinometer;
using IncliForms.Utility;
using IncliForms.Views;
using System;
using System.IO;
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

            MainPage = new AppShell();

            //var a = Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryDocuments).AbsolutePath;
            //Directory.CreateDirectory(Path.Combine(a, "incliforms"));


            //System.Collections.Generic.List<Models.Inclinometer.AdrDatablock> datablocks = null;
            //var record = new Models.Inclinometer.AdrRecord();

            //record.EndDepth = 10;

            //Random r = new Random();
            //for (float i = 0; i < record.EndDepth; i += 0.5f)
            //{
            //    datablocks.Add(new AdrDatablock() {  Aminus = r.Next()});
            //}


            //MainPage = new AdrImageGraph(record: record, datablocks: datablocks);
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
