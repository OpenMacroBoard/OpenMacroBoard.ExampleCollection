using OpenMacroBoard.SDK;
using System.Drawing;
using System.Reflection;

namespace OpenMacroBoard.Examples.Minesweeper
{
    internal class ClassicMinesweeperIconSet : IMinesweeperIconSet
    {
        private readonly KeyBitmap[] numberBitmaps = new KeyBitmap[9];
        private readonly Font font = new Font(FontFamily.GenericMonospace, 55);

        public ClassicMinesweeperIconSet()
        {
            InitializeBitmaps();
        }

        public KeyBitmap this[int number] => numberBitmaps[number];

        public KeyBitmap ExplodedMine { get; } = LoadExampleImageFromResources("mine.png");
        public KeyBitmap DefusedMine { get; } = LoadExampleImageFromResources("check.png");
        public KeyBitmap FlagWithoutMine { get; } = LoadExampleImageFromResources("cross.png");
        public KeyBitmap Flag { get; } = LoadExampleImageFromResources("flag.png");
        public KeyBitmap HiddenCell { get; } = LoadExampleImageFromResources("unknown.png");

        private void InitializeBitmaps()
        {
            for (var i = 0; i < numberBitmaps.Length; i++)
            {
                numberBitmaps[i] = KeyBitmap.Create.FromGraphics(
                    96,
                    96,
                    g =>
                    {
                        g.DrawString(
                            i.ToString(),
                            font,
                            Brushes.White,
                            10,
                            10
                        );
                    }
                );
            }

            numberBitmaps[0] = KeyBitmap.Black;
        }

        private static KeyBitmap LoadExampleImageFromResources(string name)
        {
            var asm = Assembly.GetExecutingAssembly();
            var resourceName = $"OpenMacroBoard.Examples.Minesweeper.icons.{name}";

            using var resStream = asm.GetManifestResourceStream(resourceName);
            using var bmp = (Bitmap)Image.FromStream(resStream);

            return KeyBitmap.Create.FromBitmap(bmp);
        }
    }
}
