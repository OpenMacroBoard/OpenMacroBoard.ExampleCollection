using System;
using System.Drawing;
using OpenMacroBoard.SDK;
using StreamDeckSharp.Examples.CommonStuff;

namespace StreamDeckSharp.Examples.DrawKeys
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var deck = ExampleHelper.OpenBoard())
            {
                var b = Brushes.White;
                var f = new Font("Arial", 20);

                for (int kId = 0; kId < deck.Keys.Count; kId++)
                {
                    var bmp = KeyBitmap.Create.FromGraphics(100, 100, (g) =>
                    {
                        g.DrawString($"{kId,2}", f, b, new PointF(30, 33));
                    });
                    deck.SetKeyBitmap(kId, bmp);
                }

                ExampleHelper.WaitForKeyToExit();
            }
        }
    }
}
