using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

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
            GetKeyBitmap = ReferenceImageFactory.GetChangingFilledImage;
            Console.CancelKeyPress += Console_CancelKeyPress;

            var availableFunctions = new Func<int, KeyBitmap>[]
            {
                GetBlank,
                ReferenceImageFactory.GetStableFilledImage,
                ReferenceImageFactory.GetChangingFilledImage,
                ReferenceImageFactory.GetStableLineImage,
                ReferenceImageFactory.GetChangingLineImage
            };


            using (var deck = StreamDeck.OpenDevice())
            {
                //setup StreamDeck button mapping
                keyFunctions = new Func<int, KeyBitmap>[deck.KeyCount];
                for (int i = 0; i < deck.KeyCount; i++)
                    keyFunctions[i] = availableFunctions[i % availableFunctions.Length];

                deck.KeyStateChanged += Deck_KeyStateChanged;

                while (mode == 0)
                {
                    for (int i = 0; i < deck.KeyCount; i++)
                    {
                        deck.SetKeyBitmap(i, GetKeyBitmap(i));
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

        private static void Deck_KeyStateChanged(object sender, KeyEventArgs e)
        {
            if (e.IsDown)
                GetKeyBitmap = keyFunctions[e.Key];
        }

        static KeyBitmap GetBlank(int k) => KeyBitmap.Black;
    }
}
