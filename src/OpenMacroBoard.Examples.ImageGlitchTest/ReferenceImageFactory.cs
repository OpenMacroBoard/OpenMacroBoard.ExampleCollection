using OpenMacroBoard.SDK;

namespace OpenMacroBoard.Examples.ImageGlitchTest
{
    internal static class ReferenceImageFactory
    {
        private static int pos = 0;
        private static void IncrementPos()
        {
            pos++;
            if (pos >= 10000000) pos = 0;
        }

        public static KeyBitmap GetStableFilledImage(int key)
        {
            return GetGrayImage((byte)(key * 18));
        }

        public static KeyBitmap GetStableLineImageVertical(int key)
        {
            return GetVerticalStripeImage(key * 4);
        }

        public static KeyBitmap GetStableLineImageHorizontal(int key)
        {
            return GetHorizontalStripeImage(key * 4);
        }

        public static KeyBitmap GetChangingFilledImage(int key)
        {
            IncrementPos();
            return GetGrayImage((byte)(pos % 251));
        }

        public static KeyBitmap GetChangingLineImageVertical(int key)
        {
            IncrementPos();
            return GetVerticalStripeImage(pos % 71);
        }

        public static KeyBitmap GetChangingLineImageHorizontal(int key)
        {
            IncrementPos();
            return GetHorizontalStripeImage(pos % 71);
        }

        private static KeyBitmap GetVerticalStripeImage(int pos)
        {
            var raw = new byte[72 * 72 * 3];
            for (int y = 0; y < 72; y++)
            {
                var p = (y * 72 + pos) * 3;
                raw[p + 0] = 255;
                raw[p + 1] = 255;
                raw[p + 2] = 255;
            }
            return new KeyBitmap(72, 72, raw);
        }

        private static readonly KeyBitmap rainbow = CreateRainbow();
        private static KeyBitmap CreateRainbow()
        {
            var raw = new byte[72 * 72 * 3];
            for (int y = 0; y < 72; y++)
                for (int x = 0; x < 72; x++)
                {
                    var p = (y * 72 + x) * 3;
                    byte blue = (byte)(y * 3);
                    byte yellow = (byte)(x * 3);
                    raw[p + 0] = blue;
                    raw[p + 1] = yellow;
                    raw[p + 2] = yellow;
                }

            return new KeyBitmap(72, 72, raw);
        }

        public static KeyBitmap Rainbow(int pos)
        {
            return rainbow;
        }

        private static KeyBitmap GetHorizontalStripeImage(int pos)
        {
            var raw = new byte[72 * 72 * 3];
            for (int x = 0; x < 72; x++)
            {
                var p = (pos * 72 + x) * 3;
                raw[p + 0] = 255;
                raw[p + 1] = 255;
                raw[p + 2] = 255;
            }
            return new KeyBitmap(72, 72, raw);
        }

        private static KeyBitmap GetGrayImage(byte b)
        {
            var raw = new byte[72 * 72 * 3];

            for (int i = 0; i < raw.Length; i++)
                raw[i] = b;

            return new KeyBitmap(72, 72, raw);
        }
    }
}
