using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;

namespace IncliForms.Utility
{
    public static class ExtensionMethods
    {
        /// <summary>
        /// Draw a Rectangle from the between two points
        /// </summary>
        /// <param name="map">The image to draw on</param>
        /// <param name="a">Point One</param>
        /// <param name="b">Point Two</param>
        /// <param name="color">Rectangle Fill Color</param>
        public static void Rectangle(this SKBitmap map, Point a, Point b, SKColor color)
        {
            for (int i = a.X; i < b.X; i++)
            {
                for (int j = a.Y; j < b.Y; j++)
                {
                    map.SetPixel(i, j, color);
                }
            }
        }

        /// <summary>
        /// Place a Sticker on a background image
        /// </summary>
        /// <param name="map">The background image to stick the <paramref name="sticker"/> on</param>
        /// <param name="location">Top Lef Corner of the sticker</param>
        /// <param name="sticker">The image to stick on the <paramref name="map"/></param>
        public static void Place(this SKBitmap map, Point location, SKBitmap sticker)
        {
            // Traverse inside sticker bounds
            for (int i = location.X; i < (location.X + sticker.Width); i++)
            {
                for (int j = location.Y; j < (location.Y + sticker.Height); j++)
                {
                    // Sticker color inside sticker bounds
                    SKColor A = sticker.GetPixel(i - location.X, j - location.Y);
                    // Background color inside sticker bounds
                    SKColor B = map.GetPixel(i, j);

                    map.SetPixel(i, j, A.CombineWith(B));
                }
            }

        }

        /// <summary>
        /// Combine two colors, taking into regard their transparencies
        /// </summary>
        /// <param name="color"></param>
        /// <param name="secondColor"></param>
        /// <returns></returns>
        public static SKColor CombineWith(this SKColor color, SKColor secondColor)
        {
            int rOut = (color.Red * color.Alpha / 255) + (secondColor.Red * secondColor.Alpha * (255 - color.Alpha) / (255 * 255));
            int gOut = (color.Green * color.Alpha / 255) + (secondColor.Green * secondColor.Alpha * (255 - color.Alpha) / (255 * 255));
            int bOut = (color.Blue * color.Alpha / 255) + (secondColor.Blue * secondColor.Alpha * (255 - color.Alpha) / (255 * 255));
            int aOut = color.Alpha + (secondColor.Alpha * (255 - color.Alpha) / 255);

            return new SKColor((byte)aOut, (byte)rOut, (byte)gOut, (byte)bOut);
        }


        public static void Circle(this SKBitmap map, Point location, int radius)
        {

        }

        public static void Border(this SKBitmap map, Point a, Point b, SKColor color, int thickness)
        {
            for (int q = 0; q < thickness; q++)
            {
                for (int i = a.X + q; i < b.X - q; i++)
                {
                    map.SetPixel(i, a.Y, color);
                    map.SetPixel(i, b.Y, color);
                }

                for (int i = a.Y + q; i < b.Y - q; i++)
                {
                    map.SetPixel(i, a.X, color);
                    map.SetPixel(i, b.X, color);
                }
            }
        }

        /// <summary>
        /// To stream
        /// </summary>
        /// <example>
        /// ImageSource.FromStream(() => stream);
        /// </example>
        /// <param name="bitmap"></param>
        /// <returns></returns>
        public static Stream ToImageStream(this SKBitmap bitmap)
        {
            // create an image COPY
            //SKImage image = SKImage.FromBitmap(bitmap);
            // OR
            // create an image WRAPPER
            SKImage image = SKImage.FromPixels(bitmap.PeekPixels());
            // encode the image (defaults to PNG)
            SKData encoded = image.Encode();
            // get a stream over the encoded data
            return encoded.AsStream();
        }

    }
}
