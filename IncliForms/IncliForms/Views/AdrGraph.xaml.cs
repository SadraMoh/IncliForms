using IncliForms.Models.Inclinometer;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Shapes;
using Xamarin.Forms.Xaml;

namespace IncliForms.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AdrGraph : ContentPage
    {
        public AdrRecord Record { get; set; }
        public List<AdrDatablock> Datablocks { get; set; }

        class GraphVm
        {
            public float Depth { get; set; }
            public Thickness MarginFirst { get; set; }
            public Thickness MarginSecond { get; set; }
            public float Delta { get; set; }
        }

        ObservableCollection<GraphVm> blocks = new ObservableCollection<GraphVm>();

        public AdrGraph()
        {
            InitializeComponent();
        }

        public AdrGraph(AdrRecord record, List<AdrDatablock> datablocks)
        {
            InitializeComponent();
            Record = record;
            Datablocks = datablocks;
            DrawGraph();
        }

        private void DrawGraph()
        {
            #region A
            float MaxValueA = 0;
            foreach (var block in Datablocks)
            {
                float val = (block.Aplus + block.Aminus) / 2;
                MaxValueA = (val > 0) ? MaxValueA + val : MaxValueA;
            }

            float sum = 0;
            foreach (var block in Datablocks)
            {
                float val = (block.Aplus + block.Aminus) / 2;
                sum += val;
                float marg = (sum / MaxValueA);
                //-
                var stack = new StackLayout();
                stack.Orientation = StackOrientation.Horizontal;
                Label lblDepth = new Label();
                lblDepth.WidthRequest = 36;
                lblDepth.Text = block.Depth.ToString();
                stack.Children.Add(lblDepth);
                AbsoluteLayout abs = new AbsoluteLayout();
                abs.HorizontalOptions = LayoutOptions.FillAndExpand;
                stack.Children.Add(abs);
                var cir1 = new Ellipse();
                cir1.HeightRequest = 12;
                cir1.WidthRequest = 12;
                cir1.BackgroundColor = Color.LightGray;
                AbsoluteLayout.SetLayoutFlags(cir1, AbsoluteLayoutFlags.XProportional);
                AbsoluteLayout.SetLayoutBounds(cir1, new Xamarin.Forms.Rectangle(0, 0, 12, 12));
                var cir2 = new Ellipse();
                cir2.HeightRequest = 12;
                cir2.WidthRequest = 12;
                cir2.BackgroundColor = Color.DarkGray;
                AbsoluteLayout.SetLayoutFlags(cir2, AbsoluteLayoutFlags.XProportional);
                AbsoluteLayout.SetLayoutBounds(cir2, new Xamarin.Forms.Rectangle(marg, 0, 12, 12));
                //-
                abs.Children.Add(cir1);
                abs.Children.Add(cir2);
                Label lblDelta = new Label();
                lblDelta.Text = 0.ToString();
                lblDelta.WidthRequest = 36;
                stack.Children.Add(lblDelta);
                //-
                stckA.Children.Add(stack);
            }
            #endregion

            #region B
            float MaxValueB = 0;
            foreach (var block in Datablocks)
            {
                float val = Math.Abs(block.Bplus + block.Bminus) / 2;
                MaxValueB = (val > 0) ? MaxValueB + val : MaxValueB;
            }

            sum = 0;
            foreach (var block in Datablocks)
            {
                float val = Math.Abs(block.Bplus + block.Bminus) / 2;
                sum += val;
                float marg = (sum / MaxValueB);
                //-
                var stack = new StackLayout();
                stack.Orientation = StackOrientation.Horizontal;
                Label lblDepth = new Label();
                lblDepth.WidthRequest = 36;
                lblDepth.Text = block.Depth.ToString();
                stack.Children.Add(lblDepth);
                AbsoluteLayout abs = new AbsoluteLayout();
                abs.HorizontalOptions = LayoutOptions.FillAndExpand;
                stack.Children.Add(abs);
                var cir1 = new Ellipse();
                cir1.HeightRequest = 12;
                cir1.WidthRequest = 12;
                cir1.BackgroundColor = Color.LightGray;
                AbsoluteLayout.SetLayoutFlags(cir1, AbsoluteLayoutFlags.XProportional);
                AbsoluteLayout.SetLayoutBounds(cir1, new Xamarin.Forms.Rectangle(0, 0, 12, 12));
                var cir2 = new Ellipse();
                cir2.HeightRequest = 12;
                cir2.WidthRequest = 12;
                cir2.BackgroundColor = Color.DarkGray;
                AbsoluteLayout.SetLayoutFlags(cir2, AbsoluteLayoutFlags.XProportional);
                AbsoluteLayout.SetLayoutBounds(cir2, new Xamarin.Forms.Rectangle(marg, 0, 12, 12));
                //-
                abs.Children.Add(cir1);
                abs.Children.Add(cir2);
                Label lblDelta = new Label();
                lblDelta.Text = marg.ToString();
                lblDelta.WidthRequest = 36;
                stack.Children.Add(lblDelta);
                //-
                stckB.Children.Add(stack);
            }
            #endregion
        }
    }
}