using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using OpenMacroBoard.SDK;
using StreamDeckSharp.Examples.CommonStuff;

namespace StreamDeckSharp.Examples.ImageGlitchTest
{
    class Program
    {
        private static Func<int, KeyBitmap> GetKeyBitmap = null;
        private static Func<int, KeyBitmap>[] keyFunctions;

        //=====================================
        //  Epilepsiewarning! Flicking images
        //=====================================
        static void Main(string[] args)
        {
            //default (startup) value
            //press buttons on streamdeck to change method
            GetKeyBitmap = ReferenceImageFactory.Rainbow;
            Console.CancelKeyPress += Console_CancelKeyPress;

            var availableFunctions = new Func<int, KeyBitmap>[]
            {
                ReferenceImageFactory.GetStableLineImageHorizontal,
                ReferenceImageFactory.GetStableLineImageVertical,
                ReferenceImageFactory.GetChangingLineImageVertical,
                ReferenceImageFactory.GetChangingLineImageHorizontal,
                ReferenceImageFactory.GetChangingFilledImage,
                ReferenceImageFactory.GetStableFilledImage,
                GetBlank,
            };

            var rb = GetKeyBitmap(0);

            
            using (var deck = ExampleHelper.OpenBoard())
            {
                //setup StreamDeck button mapping
                keyFunctions = new Func<int, KeyBitmap>[deck.Keys.Count];
                for (int i = 0; i < deck.Keys.Count; i++)
                    keyFunctions[i] = availableFunctions[i % availableFunctions.Length];

                deck.KeyStateChanged += Deck_KeyStateChanged;

                while (mode == 0)
                {
                    for (int i = 0; i < deck.Keys.Count; i++)
                    {
                        var bm = GetKeyBitmap(i);
                        deck.SetKeyBitmap(i, bm);
                    }
                }
            }
        }

        private static volatile int mode = 0;
        private static void Console_CancelKeyPress(object sender, ConsoleCancelEventArgs e)
        {
            e.Cancel = true;
            Interlocked.Increment(ref mode);
        }

        private static readonly Random rnd = new Random();
        private static void Deck_KeyStateChanged(object sender, KeyEventArgs e)
        {
            GetKeyBitmap = keyFunctions[e.Key];
        }

        static KeyBitmap GetBlank(int k) => KeyBitmap.Black;
    }
}
