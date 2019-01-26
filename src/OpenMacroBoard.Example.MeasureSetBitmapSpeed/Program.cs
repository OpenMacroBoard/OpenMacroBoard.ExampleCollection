using System;
using System.Diagnostics;
using OpenMacroBoard.SDK;
using StreamDeckSharp.Examples.CommonStuff;

namespace StreamDeckSharp.Examples.MeasureSetBitmapSpeed
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var deck = ExampleHelper.OpenBoard())
            {
                var sw = Stopwatch.StartNew();

                //Create random noise image
                var rnd = new Random();
                var raw = new byte[72 * 72 * 3];
                rnd.NextBytes(raw);
                var rndImage = new KeyBitmap(72, 72, raw);

                deck.ClearKeys();

                //Run a few SetKeyBitmaps
                long cnt = 50_000;
                long i = cnt;
                while (--i > 0)
                {
                    deck.SetKeyBitmap(0, rndImage);
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
