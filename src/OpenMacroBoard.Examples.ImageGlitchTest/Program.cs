using OpenMacroBoard.Examples.CommonStuff;
using OpenMacroBoard.SDK;
using StreamDeckSharp;
using System;

namespace OpenMacroBoard.Examples.ImageGlitchTest
{
    internal class Program
    {
        private static bool exitRequested = false;
        private static bool readGlitchEnabled = true;

        private static void Main()
        {
            Console.CancelKeyPress += Console_CancelKeyPress;

            using (var deck = ExampleHelper.OpenBoard())
            {
                var imgFactory = new ReferenceImageFactory(deck.GetDeviceImageSize(), deck.Keys.Count);
                var readerAction = GetReadAction(deck);

                Console.WriteLine("Use Numpad + and - to iterate through different patterns and Return to toggle ReadGlitch.");
                Console.WriteLine();

                while (!exitRequested)
                {
                    for (var i = 0; i < deck.Keys.Count; i++)
                    {
                        deck.SetKeyBitmap(i, imgFactory.GetKeyBitmap(i));

                        if (readGlitchEnabled)
                        {
                            readerAction();
                        }
                    }

                    ProcessConsoleKeys(imgFactory);
                }
            }
        }

        private static void ProcessConsoleKeys(ReferenceImageFactory imgFactory)
        {
            if (Console.KeyAvailable)
            {
                var k = Console.ReadKey(true);

                if (k.Key == ConsoleKey.Add)
                {
                    imgFactory.CurrentMode++;
                }
                else if (k.Key == ConsoleKey.Subtract)
                {
                    imgFactory.CurrentMode--;
                }
                else if (k.Key == ConsoleKey.Enter)
                {
                    readGlitchEnabled = !readGlitchEnabled;
                }

                Console.WriteLine($"Pattern ID: {imgFactory.CurrentMode,2}{(readGlitchEnabled ? " + GLITCH" : string.Empty)}");
            }
        }

        private static Action GetReadAction(IMacroBoard deck)
        {
            if (deck is IStreamDeckBoard streamDeck)
            {
                return () => streamDeck.GetSerialNumber();
            }
            else
            {
                return () => { };
            }
        }

        private static void Console_CancelKeyPress(object sender, ConsoleCancelEventArgs e)
        {
            e.Cancel = true;
            exitRequested = true;
        }
    }
}
