using System;
using System.Drawing;
using StreamDeckSharp.Extensions;

namespace StreamDeckSharp.Examples.DrawFullScreen
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var deck = StreamDeck.OpenDevice())
            using (var bmp = (Bitmap)Image.FromFile(@"C:\testimage.png"))
            {
                deck.DrawFullScreenBitmap(bmp);
                Console.ReadKey();
            }
        }
    }
}
