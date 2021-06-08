using Android.App;
using Android.Content;
using Android.Media;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IncliForms.Droid;
using Xamarin.Forms;

[assembly: Dependency(typeof(Audio))]
namespace IncliForms.Droid
{
    public class Audio : IncliForms.Utility.IAudio
    {
        /// <summary>
        /// Play an .mp3 file residing in the Android Assets folder
        /// </summary>
        /// <param name="fileName">Path format: Soundtracks/filename.mp3</param>
        /// Throws and catches a 
        /// <exception cref="Java.IO.FileNotFoundException"></exception>
        /// if the path is invalid
        public void PlayAudioFile(string fileName)
        {
            try
            {
                var player = new MediaPlayer();
                var fd = global::Android.App.Application.Context.Assets.OpenFd(fileName);
                player.Prepared += (s, e) =>
                {
                    player.Start();
                };
                player.SetDataSource(fd.FileDescriptor, fd.StartOffset, fd.Length);
                player.Prepare();
            }
            catch (Java.IO.FileNotFoundException)
            {
                var player = new MediaPlayer();
                var fd = global::Android.App.Application.Context.Assets.OpenFd("Soundtracks/Alert.mp3");
                player.Prepared += (s, e) =>
                {
                    player.Start();
                };
                player.SetDataSource(fd.FileDescriptor, fd.StartOffset, fd.Length);
                player.Prepare();
            }
        }
    }
}