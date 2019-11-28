using OpenMacroBoard.Examples.CommonStuff;
using OpenMacroBoard.SDK;
using System.Drawing;

namespace OpenMacroBoard.Examples.DrawKeys
{
    internal class Program
    {
        private static void Main()
        {
            using (var deck = ExampleHelper.OpenBoard())
            {
                var lastActive = -1;
                deck.KeyStateChanged += (s, e) =>
                {
                    if (!e.IsDown)
                        return;

                    var newId = e.Key;

                    if (lastActive == newId)
                        newId = -1;

                    DrawKeys(deck, newId);
                    lastActive = newId;
                };

                DrawKeys(deck);
                ExampleHelper.WaitForKeyToExit();
            }
        }

        private static void DrawKeys(IMacroBoard deck, int activeKey = -1)
        {
            var b = Brushes.White;
            var f = new Font("Arial", 20);
            var fb = new Font("Arial", 60, FontStyle.Bold);

            for (int kId = 0; kId < deck.Keys.Count; kId++)
            {
                var font = f;
                var origin = new PointF(30, 33);

                if (kId == activeKey)
                {
                    font = fb;
                    origin = new PointF(-6, 5);
                }

                var bmp = KeyBitmap.Create.FromGraphics(100, 100, (g) =>
                {
                    g.DrawString($"{kId,2}", font, b, origin);
                });

                deck.SetKeyBitmap(kId, bmp);
            }
        }
    }
}
