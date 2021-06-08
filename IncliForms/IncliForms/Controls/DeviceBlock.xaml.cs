using IncliForms.Models;
using IncliForms.Models.Inclinometer;
using IncliForms.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace IncliForms.Controls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DeviceBlock : ContentView
    {

        public delegate void ClickedHandler(object sender, EventArgs e);
        public event ClickedHandler Clicked;

        public AdrInclinometer Inclinometer { get; }

        public class vm : INotifyPropertyChanged
        {
            public ICommand cmdClick { get; set; }


            public delegate void ClickedHandler(object sender, EventArgs e);
            public event ClickedHandler Clicked;

            public event PropertyChangedEventHandler PropertyChanged;

            public vm()
            {
                cmdClick = new Command(
               execute: () => { Clicked.Invoke(this, null); }
               , canExecute: () => true
               );
            }

        }

        public vm Vm { get; set; }

        public DeviceBlock()
        {
            InitializeComponent();
        }

        public DeviceBlock(AdrInclinometer model)
        {
            InitializeComponent();
            this.Inclinometer = model;
            lblTitle.Text = model.BoreholeName;
            lblModel.Text = model.ModelName;
            lblVersion.Text = model.VersionName;

            Vm = new vm();
            this.BindingContext = Vm;

            Vm.Clicked += (sender, e) => { TapGestureRecognizer_Tapped(this, null); };
        }



        private void btnSettings_Tapped(object sender, EventArgs e)
        {
            Navigation.PushAsync(new AdrSettings(Inclinometer));
        }

        private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            Clicked?.Invoke(this, e);
        }
    }
}