using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StreamDeckSharp.Examples.ImageGlitchTest
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

        public static KeyBitmap GetStableLineImage(int key)
        {
            return GetStripeImage(key * 4);
        }

        public static KeyBitmap GetChangingFilledImage(int key)
        {
            IncrementPos();
            return GetGrayImage((byte)(pos % 251));
        }

        public static KeyBitmap GetChangingLineImage(int key)
        {
            IncrementPos();
            return GetStripeImage(pos % 71);
        }

        private static KeyBitmap GetStripeImage(int pos)
        {
            var raw = new byte[72 * 72 * 3];
            for (int y = 0; y < 72; y++)
            {
                var p = (y * 72 + pos) * 3;
                raw[p + 0] = 255;
                raw[p + 1] = 255;
                raw[p + 2] = 255;
            }
            return KeyBitmap.FromRawBitmap(raw);
        }

        private static KeyBitmap GetGrayImage(byte b)
        {
            var raw = new byte[72 * 72 * 3];

            for (int i = 0; i < raw.Length; i++)
                raw[i] = b;

            return KeyBitmap.FromRawBitmap(raw);
        }
    }
}
