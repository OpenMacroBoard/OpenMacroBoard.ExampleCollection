using OpenMacroBoard.Examples.CommonStuff;
using OpenMacroBoard.SDK;
using System;

namespace OpenMacroBoard.Examples.Rainbow
{
    internal class Program
    {
        private static readonly Random rnd = new Random();
        private static readonly byte[] rgbBuffer = new byte[3];

        private static int firstKeyId = 0;
        private static int lastKeyId = 0;

        private static bool firstKeyPressed = false;
        private static bool lastKeyPressed = false;

        private static void Main()
        {
            using (var deck = ExampleHelper.OpenBoard())
            {
                deck.SetBrightness(100);
                lastKeyId = deck.Keys.Count - 1;

                Console.WriteLine("INFO: Press some keys on the Stream Deck.");
                deck.ClearKeys();
                deck.KeyStateChanged += Deck_KeyPressed;
                Console.WriteLine();

                ExampleHelper.WaitForKeyToExit();
            }
        }

        private static void Deck_KeyPressed(object sender, KeyEventArgs e)
        {
            if (!(sender is IMacroBoard d))
            {
                return;
            }

            if (e.IsDown)
            {
                d.SetKeyBitmap(e.Key, GetRandomColorImage());
            }

            if (e.Key == firstKeyId)
            {
                firstKeyPressed = e.IsDown;
            }

            if (e.Key == lastKeyId)
            {
                lastKeyPressed = e.IsDown;
            }

            if (firstKeyPressed && lastKeyPressed)
            {
                d.ClearKeys();
            }
        }

        private static KeyBitmap GetRandomColorImage()
        {
            rnd.NextBytes(rgbBuffer);
            return KeyBitmap.Create.FromRgb(rgbBuffer[0], rgbBuffer[1], rgbBuffer[2]);
        }
    }
}
