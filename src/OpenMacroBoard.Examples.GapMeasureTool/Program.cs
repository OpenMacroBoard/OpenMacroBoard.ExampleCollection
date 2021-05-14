using OpenMacroBoard.Examples.CommonStuff;
using OpenMacroBoard.SDK;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

namespace OpenMacroBoard.Examples.GapMeasureTool
{
    class Program
    {
        private static void Main()
        {
            // Calibration tip: A rectangle spanning over a single gap should be the same length
            // when moved to span two gaps. If it's not tweak the gap value inside the HardwareInfo classes.

            using (var deck = ExampleHelper.OpenBoard())
            {
                Console.WriteLine("Use WASD and IJKL to move and resize the rectangle and press Q to quit.");
                Console.WriteLine();

                var rect = deck.Keys[deck.Keys.Count / 2];
                var keyArea = deck.Keys.Area;
                var fullScreenBmp = new Bitmap(keyArea.Width, keyArea.Height, PixelFormat.Format24bppRgb);

                while (true)
                {
                    using (var g = Graphics.FromImage(fullScreenBmp))
                    {
                        g.InterpolationMode = InterpolationMode.Bilinear;
                        g.CompositingMode = CompositingMode.SourceCopy;
                        g.CompositingQuality = CompositingQuality.HighQuality;
                        g.SmoothingMode = SmoothingMode.AntiAlias;

                        g.FillRectangle(Brushes.Black, keyArea);
                        g.FillRectangle(Brushes.White, rect);
                    }

                    deck.DrawFullScreenBitmap(fullScreenBmp);
                    var k = Console.ReadKey(true);

                    if (k.Key == ConsoleKey.Q)
                    {
                        break;
                    }

                    if (k.Key == ConsoleKey.A)
                    {
                        rect.X--;
                    }
                    else if (k.Key == ConsoleKey.D)
                    {
                        rect.X++;
                    }
                    else if (k.Key == ConsoleKey.W)
                    {
                        rect.Y--;
                    }
                    else if (k.Key == ConsoleKey.S)
                    {
                        rect.Y++;
                    }
                    else if (k.Key == ConsoleKey.J)
                    {
                        rect.Width--;
                    }
                    else if (k.Key == ConsoleKey.L)
                    {
                        rect.Width++;
                    }
                    else if (k.Key == ConsoleKey.I)
                    {
                        rect.Height++;
                    }
                    else if (k.Key == ConsoleKey.K)
                    {
                        rect.Height--;
                    }
                }
            }
        }
    }
}
