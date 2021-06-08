using Android.Bluetooth;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace IncliForms.Utility
{
    public interface IBluetoothSerial
    {
        BluetoothAdapter Adapter { get; }
        BluetoothDevice Device { get; }
        BluetoothSocket Socket { get; }
        void initAdapter();
        List<BluetoothDevice> GetNearbyDevices();
        Task<BluetoothDevice> ConnectToDevice(string name = null);
        bool Disconnect();
        Task<string> ReadData();
        void WriteData(byte[] buffer);
    }
}
