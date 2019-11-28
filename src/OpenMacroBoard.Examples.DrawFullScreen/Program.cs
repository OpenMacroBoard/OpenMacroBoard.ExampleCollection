using OpenMacroBoard.Examples.CommonStuff;
using OpenMacroBoard.SDK;
using System.Drawing;
using System.Reflection;

namespace OpenMacroBoard.Examples.DrawFullScreen
{
    internal class Program
    {
        private static void Main()
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
