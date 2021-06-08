using IncliForms.Models.Inclinometer;
using IncliForms.Utility;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace IncliForms.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AdrImageGraph : ContentPage
    {
        public AdrRecord Record { get; set; }
        public List<AdrDatablock> Datablocks { get; set; }

        public AdrImageGraph()
        {
            InitializeComponent();
        }

        public AdrImageGraph(AdrRecord record, List<AdrDatablock> datablocks)
        {
            InitializeComponent();
            Record = record;
            Datablocks = datablocks;
            DrawGraph();
        }

        readonly static int cellHeight = 24;


        /// <summary>
        /// Positive sum of positive inclinations
        /// </summary>
        double plusSum = 0;
        /// <summary>
        /// Positive sum of negative inclinations
        /// </summary>
        double minusSum = 0;

        /// <summary>
        /// Greatest Positive inclination
        /// </summary>
        double plusMax = 0;
        /// <summary>
        /// Greatest Negative inclination 
        /// </summary>
        double minusMax = 0;

        private void DrawGraph()
        {
            plusSum = Math.Abs(Datablocks.Where(x => x.AvA() > 0).Sum(x => x.AvA()));
            minusSum = Math.Abs(Datablocks.Where(x => x.AvA() < 0).Sum(x => x.AvA()));

            plusMax = Datablocks.OrderBy(x => x.AvA()).First().AvA();
            minusMax = Datablocks.OrderByDescending(x => x.AvA()).First().AvA();


            double imageWidth = (plusMax * 10) + (minusMax * 10);

            double imageHeight = Datablocks.Count * 100;

            SKBitmap map = new SKBitmap((int)imageWidth, (int)imageHeight);
            for (int i = 0; i < map.Width; i++)
                for (int j = 0; j < map.Height; j++)
                    map.SetPixel(i, j, SKColor.Parse("#fff"));


            SKCanvas canvas = new SKCanvas(map);

            SKPaint paint = new SKPaint();
            paint.Color = SKColor.Parse("#000");
            paint.StrokeWidth = 5;

            SKPaint point = new SKPaint();
            point.Color = SKColor.Parse("#555");
            point.StrokeCap = SKStrokeCap.Round;
            paint.StrokeWidth = 5;

            canvas.DrawLine(new SKPoint(0, 0), new SKPoint(0, map.Height), paint);
            canvas.DrawLine(new SKPoint(0, map.Height), new SKPoint(map.Width, map.Height), paint);

            for (int i = 0; i < Datablocks.Count; i++)
            {
                var block = Datablocks[i];
                canvas.DrawCircle(new SKPoint(block.AvA() * 10, i * 64), 6 , point);
            }

            imgMain.Source = ImageSource.FromStream(() => map.ToImageStream());

        }
    }
}