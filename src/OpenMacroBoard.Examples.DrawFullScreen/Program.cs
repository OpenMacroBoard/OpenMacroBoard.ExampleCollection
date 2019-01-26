using System.Drawing;
using System.Reflection;
using OpenMacroBoard.SDK;
using OpenMacroBoard.Examples.CommonStuff;

namespace OpenMacroBoard.Examples.DrawFullScreen
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var deck = ExampleHelper.OpenBoard())
            using (var bmp = LoadExampleImageFromResources())
            {
                deck.DrawFullScreenBitmap(bmp);
                ExampleHelper.WaitForKeyToExit();
            }
        }

        private static Bitmap LoadExampleImageFromResources()
        {
            var asm = Assembly.GetExecutingAssembly();
            var resourceName = "OpenMacroBoard.Examples.DrawFullScreen.ExampleImage.jpg";

            using (var resStream = asm.GetManifestResourceStream(resourceName))
            {
                return (Bitmap)Image.FromStream(resStream);
            }
        }
    }
}
