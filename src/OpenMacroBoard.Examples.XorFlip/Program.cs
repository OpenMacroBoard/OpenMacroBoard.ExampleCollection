using OpenMacroBoard.Examples.CommonStuff;

namespace OpenMacroBoard.Examples.XorFlip
{
    class Program
    {
        static void Main(string[] args)
        {
            using var deck = ExampleHelper.OpenBoard();

            var host = new MacroBoardHost(deck);
            var game = new XorFlipGame(host);

            ExampleHelper.WaitForKeyToExit();
        }
    }
}
