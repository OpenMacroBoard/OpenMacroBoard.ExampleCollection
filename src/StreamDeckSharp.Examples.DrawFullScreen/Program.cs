using System;
using System.Drawing;
using System.Reflection;
using OpenMacroBoard.SDK;
using StreamDeckSharp.Examples.CommonStuff;

namespace StreamDeckSharp.Examples.DrawFullScreen
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var deck = ExampleHelper.OpenBoard())
            using (var bmp = LoadExampleImageFromResources())
            {
                deck.DrawFullScreenBitmap(bmp);
                Console.ReadKey();
            }
        }

        private static Bitmap LoadExampleImageFromResources()
        {
            var asm = Assembly.GetExecutingAssembly();
            var resourceName = "StreamDeckSharp.Examples.DrawFullScreen.ExampleImage.jpg";

            using (var resStream = asm.GetManifestResourceStream(resourceName))
            {
                return (Bitmap)Image.FromStream(resStream);
            }
        }
    }
}
