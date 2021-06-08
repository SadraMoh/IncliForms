using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace IncliForms.Controls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PillButton : ContentView
    {

        //public delegate void ClickedHandler(object sender, EventArgs e);
        //public event ClickedHandler Clicked;

        //public string CornerRadius { get => CornerRadius.ToString();
        //    set => cornerRadius = new CornerRadius(CornerRadius.Split(" ")[0]); }
        //public CornerRadius cornerRadius;
        //public string PillPadding { get => PillPadding.ToString(); set => pillPadding = float.Parse(value); }
        //public Thickness pillPadding;
        //public string PillWidth { get => PillWidth.ToString(); set => pillWidth = float.Parse(value); }
        //public float pillWidth;
        //public string PillHeight { get => PillHeight.ToString(); set => pillHeight = float.Parse(value); }
        //public float pillHeight;

        //public PillButton()
        //{
        //    InitializeComponent();
        //    main.Layout(Rectangle.FromLTRB
        //        (0, 0, PillWidth == 0 ? 42f : PillWidth, PillHeight == 0 ? 42f : PillHeight));
        //    main.Padding = PillPadding == null ? new Thickness(8) : PillPadding;
        //    main.CornerRadius = CornerRadius == 0 ? 21f : CornerRadius;
        //}

        //private void Tapped(object sender, EventArgs e)
        //{
        //    Clicked.Invoke(this, e);
        //}
    }
}