using OpenMacroBoard.SDK;
using OpenMacroBoard.VirtualBoard;

namespace StreamDeckSharp.Examples.CommonStuff
{
    public static class ExampleHelper
    {
        /// <summary>
        /// Searches for a real classic stream deck or creates a virtual one.
        /// All examples are designed for the 5x3 classic StreamDeck.
        /// </summary>
        /// <returns></returns>
        public static IMacroBoard OpenBoard()
        {
            //var realDeck = StreamDeck.EnumerateDevices(Hardware.StreamDeck).FirstOrDefault();

            //if (!(realDeck is null))
            //    return realDeck.Open();

            return BoardFactory.SpawnVirtualBoard(Hardware.StreamDeck.Keys);
        }
    }
}
