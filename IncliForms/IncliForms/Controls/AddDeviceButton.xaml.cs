using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace IncliForms.Controls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddDeviceButton : ContentView
    {
        public delegate void ClickedHandler(object sender, EventArgs e);
        public event ClickedHandler Clicked;

        public AddDeviceButton()
        {
            InitializeComponent();
        }

        private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            Clicked?.Invoke(this, e);
        }
    }
}