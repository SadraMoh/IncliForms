using IncliForms.Views;
using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace IncliForms
{
    public partial class AppShell : Xamarin.Forms.Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(Devices), typeof(Devices));
            Routing.RegisterRoute(nameof(AddDevice), typeof(AddDevice));
            Routing.RegisterRoute(nameof(AdrCalibration), typeof(AdrCalibration));
            Routing.RegisterRoute(nameof(AdrHarvest), typeof(AdrHarvest));
            Routing.RegisterRoute(nameof(AdrSettings), typeof(AdrSettings));
            Routing.RegisterRoute(nameof(Connecting), typeof(Connecting));
            Routing.RegisterRoute(nameof(ConnectionFailed), typeof(ConnectionFailed));

        }

        private async void OnMenuItemClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("//AdrHarvest");
        }
    }
}
