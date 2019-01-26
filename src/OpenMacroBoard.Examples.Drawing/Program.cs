using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Windows.Controls;
using OpenMacroBoard.SDK;
using StreamDeckSharp.Examples.CommonStuff;

namespace StreamDeckSharp.Examples.Drawing
{
    class Program
    {
        const int kSize = 100;

        [STAThread]
        static void Main(string[] args)
        {
            using (var deck = ExampleHelper.OpenBoard())
            {
                ConsoleWriteAndWait("Press any key to run System.Drawing example");
                ExampleWithSystemDrawing(deck);

                ConsoleWriteAndWait("Press any key to run WPF FrameworkElement example");
                ExampleWithWpfElement(deck);

                ExampleHelper.WaitForKeyToExit();
            }
        }

        static void ExampleWithSystemDrawing(IMacroBoard deck)
        {
            //Create a key with lambda graphics
            var key = KeyBitmap.Create.FromGraphics(kSize, kSize, g =>
            {
                //See https://stackoverflow.com/questions/6311545/c-sharp-write-text-on-bitmap for details
                g.SmoothingMode = SmoothingMode.AntiAlias;
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.PixelOffsetMode = PixelOffsetMode.HighQuality;
                g.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;

                //Fill background black
                g.FillRectangle(Brushes.Black, 0, 0, kSize, kSize);

                //Write text to graphics
                var f = new Font("Arial", 13);
                g.DrawString("Drawing", f, Brushes.White, new PointF(5, 20));
            });

            deck.SetKeyBitmap(7, key);
        }

        static void ExampleWithWpfElement(IMacroBoard deck)
        {
            var c = new Canvas
            {
                Width = kSize,
                Height = kSize,
                Background = System.Windows.Media.Brushes.Black
            };

            var t = new TextBlock
            {
                Text = "WPF",
                FontFamily = new System.Windows.Media.FontFamily("Arial"),
                FontSize = 13,
                Foreground = System.Windows.Media.Brushes.White
            };

            Canvas.SetLeft(t, 10);
            Canvas.SetTop(t, 10);

            c.Children.Add(t);

            var k = KeyBitmap.Create.FromWpfElement(kSize, kSize, c);
            deck.SetKeyBitmap(7, k);
        }

        static void ConsoleWriteAndWait(string text)
        {
            Console.WriteLine(text);
            Console.ReadKey();
        }
    }
}
