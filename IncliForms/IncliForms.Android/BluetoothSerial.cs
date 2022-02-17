using Android.App;
using Android.Content;
using Android.Database;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Java.Interop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IncliForms.Utility;
using Xamarin.Forms;
using Android.Bluetooth;
using Java.Util;
using System.Threading.Tasks;
using System.Threading;
using IncliForms.Droid;

[assembly: Dependency(typeof(BluetoothSerial))]
namespace IncliForms.Droid
{
    public class BluetoothSerial : IBluetoothSerial
    {
        public BluetoothAdapter Adapter { get; private set; }
        public BluetoothDevice Device { get; private set; }
        public BluetoothSocket Socket { get; private set; }
        public void initAdapter()
        {
            Adapter = BluetoothAdapter.DefaultAdapter;
            if (Adapter == null)
                throw new Exception("This Device does not support Bluetooth");

            if (!Adapter.IsEnabled)
                Adapter.Enable();
        }
        public List<BluetoothDevice> GetNearbyDevices()
        {
            if (Adapter is null || !Adapter.IsEnabled)
                initAdapter();

            var res = Adapter.BondedDevices.ToList();

            if (res is null)
                res = new List<BluetoothDevice>();

            return res;
        }
        public async Task<BluetoothDevice> ConnectToDevice(string name = null)
        {
            if (Adapter is null || !Adapter.IsEnabled)
            {
                initAdapter();
                while (!Adapter.IsEnabled)
                {
                    Thread.Sleep(10);
                };
            }

            if (string.IsNullOrEmpty(name))
                Device =
                (from bd in Adapter.BondedDevices
                 select bd).FirstOrDefault();
            else
                Device =
                (from bd in Adapter.BondedDevices
                 where bd.Name == name
                 select bd).FirstOrDefault();

            if (Device == null)
                throw new Exception("Named device not found.");

            Socket = Device.CreateRfcommSocketToServiceRecord(
             UUID.FromString("00001101-0000-1000-8000-00805f9b34fb"));
            await Socket.ConnectAsync();

            return Device;
        }
        public bool Disconnect()
        {
            if (Adapter is null || !Adapter.IsEnabled)
                return true;

            if (Device is null || Device == null)
                return true;

            if (Socket is null || Socket == null)
                return true;

            Socket.Close();

            return true;
        }

        public async Task<string> ReadData()
        {
            if (Adapter is null || !Adapter.IsEnabled)
                initAdapter();

            if (Device is null)
                await ConnectToDevice();

            byte[] buffer = new byte[64];


            // Read data from the device
            int res = await Socket.InputStream.ReadAsync(buffer, 0, buffer.Length);
            var str = Encoding.Default.GetString(buffer);

            return str;
        }
        public async void WriteData(byte[] buffer)
        {
            if (Adapter is null || !Adapter.IsEnabled)
                initAdapter();

            if (Device is null)
                await ConnectToDevice();

            // Write data to the device
            await Socket.OutputStream.WriteAsync(buffer, 0, buffer.Length);
        }
    }
}