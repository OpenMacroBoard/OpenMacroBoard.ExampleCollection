using OpenMacroBoard.Examples.CommonStuff;
using OpenMacroBoard.SDK;
using System;
using System.Drawing;

namespace OpenMacroBoard.Examples.ButtonPressDiagnostics
{
    class Program
    {
        private static int[] counter;

        static void Main(string[] args)
        {
            using var deck = ExampleHelper.OpenBoard();

            deck.SetBrightness(100);
            counter = new int[deck.Keys.Count];

            Console.WriteLine("INFO: Press some keys on the Stream Deck.");
            deck.ClearKeys();
            deck.KeyStateChanged += Deck_KeyPressed;
            Console.WriteLine();

            Console.WriteLine("Options: (q)uit, (c)lear, (r)eset");

            while (true)
            {
                var key = Console.ReadKey(true);

                switch (key.Key)
                {
                    case ConsoleKey.Q: return;

                    case ConsoleKey.C:
                        deck.ClearKeys();
                        break;

                    case ConsoleKey.R:
                        deck.ClearKeys();
                        deck.SetBrightness(100);
                        Array.Clear(counter, 0, counter.Length);
                        break;
                }
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
                counter[e.Key]++;
            }

            var k = KeyBitmap.Create.FromGraphics(96, 96, g =>
            {
                g.Clear(Color.Black);

                if (e.IsDown)
                {
                    g.FillEllipse(Brushes.White, new RectangleF(60, 12, 10, 10));
                }

                g.DrawString(counter[e.Key].ToString(), new Font("Arial", 12), Brushes.White, new PointF(10, 10));
            });

            d.SetKeyBitmap(e.Key, k);
        }
    }
}
