using System;
using System.Threading;
using OpenMacroBoard.SDK;
using OpenMacroBoard.Examples.CommonStuff;

namespace OpenMacroBoard.Examples.ImageGlitchTest
{
    class Program
    {
        private static volatile int mode = 0;

        static void Main()
        {
            Console.CancelKeyPress += Console_CancelKeyPress;

            using (var deck = ExampleHelper.OpenBoard())
            {
                var imgFactory = new ReferenceImageFactory(GetDeviceImageSize(deck), deck.Keys.Count);

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

        private static int GetDeviceImageSize(IMacroBoard deck)
        {
            if (!(deck.Keys is GridKeyPositionCollection gridKeys))
                throw new NotSupportedException("Device is not supported");

            if (gridKeys.KeyWidth != gridKeys.KeyHeight)
                throw new NotSupportedException("Device is not supported");

            return gridKeys.KeyWidth;
        }


        private static void Console_CancelKeyPress(object sender, ConsoleCancelEventArgs e)
        {
            e.Cancel = true;
            Interlocked.Increment(ref mode);
        }
    }
}
