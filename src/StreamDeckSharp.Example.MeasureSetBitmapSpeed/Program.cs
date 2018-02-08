using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace StreamDeckSharp.Example.MeasureSetBitmapSpeed
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var deck = StreamDeck.OpenDevice())
            {
                var sw = Stopwatch.StartNew();

                //Create random noise image
                var rnd = new Random();
                var raw = new byte[72 * 72 * 3];
                rnd.NextBytes(raw);
                var rndImage = KeyBitmap.FromRawBitmap(raw);

                deck.ClearKeys();

                //Run a few million SetKeyBitmaps
                long cnt = 5_000_000;
                long i = cnt;
                while (--i > 0)
                {
                    deck.SetKeyBitmap(7, rndImage);
                }

                var t = sw.Elapsed.TotalSeconds;
                var setKeyTime = t / cnt;

                
                //about 0.5µs on my machine
                Console.WriteLine((setKeyTime * 1000000.0) + " µs");

                //Make sure that the test takes a least 3 seconds on your machine
                // -> change cnt!
                Console.WriteLine("Total Test time: " + t + " s");

                Console.ReadKey();
            }
        }
    }
}
