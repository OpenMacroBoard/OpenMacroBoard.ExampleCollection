using OpenMacroBoard.Examples.CommonStuff;
using OpenMacroBoard.SDK;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;

namespace OpenMacroBoard.Examples.Drawing
{
    class Program
    {
        const int kSize = 100;

        [STAThread]
        static void Main()
        {
            using (var deck = ExampleHelper.OpenBoard())
            {
                ExampleWithSystemDrawing(deck);
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

            deck.SetKeyBitmap(key);
        }
    }
}
