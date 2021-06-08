using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace IncliForms.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ConnectionFailed : ContentPage
    {
        Connecting Connecting;

        public ConnectionFailed()
        {
            InitializeComponent();
        }

        public ConnectionFailed(Connecting connecting)
        {
            InitializeComponent();
            this.Connecting = connecting;
        }

        protected override bool OnBackButtonPressed()
        {
            Connecting.ConnectToDevice();
            Navigation.PopAsync();
            return base.OnBackButtonPressed();
        }

        private void btnTryAgain_Clicked(object sender, EventArgs e)
        {
            Connecting.ConnectToDevice();
            Navigation.PopAsync();
        }

        private void btnChangeDevice_Clicked(object sender, EventArgs e)
        {
            Navigation.PopToRootAsync();
        }
    }
}