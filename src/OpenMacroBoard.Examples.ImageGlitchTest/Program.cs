using OpenMacroBoard.Examples.CommonStuff;
using System;
using System.Threading;

namespace OpenMacroBoard.Examples.ImageGlitchTest
{
    internal class Program
    {
        private static volatile int mode = 0;

        private static void Main()
        {
            Console.CancelKeyPress += Console_CancelKeyPress;

            using (var deck = ExampleHelper.OpenBoard())
            {
                var imgFactory = new ReferenceImageFactory(deck.GetDeviceImageSize(), deck.Keys.Count);

                deck.KeyStateChanged += (sender, e) =>
                {
                    if (!e.IsDown)
                        return;

                    if (e.Key % 2 == 1)
                        imgFactory.CurrentMode++;
                    else
                        imgFactory.CurrentMode--;
                };

                while (mode == 0)
                {
                    for (int i = 0; i < deck.Keys.Count; i++)
                        deck.SetKeyBitmap(i, imgFactory.GetKeyBitmap(i));
                }
            }
        }

        private static void Console_CancelKeyPress(object sender, ConsoleCancelEventArgs e)
        {
            e.Cancel = true;
            Interlocked.Increment(ref mode);
        }
    }
}
