using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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

                while (true)
                {
                    for (int i = 0; i < deck.KeyCount; i++)
                    {
                        deck.SetKeyBitmap(i, GetKeyBitmap(i));
                    }
                }
            }
        }

        private static void Deck_KeyStateChanged(object sender, KeyEventArgs e)
        {
            if (e.IsDown)
                GetKeyBitmap = keyFunctions[e.Key];
        }

        static KeyBitmap GetBlank(int k) => KeyBitmap.Black;
    }
}
