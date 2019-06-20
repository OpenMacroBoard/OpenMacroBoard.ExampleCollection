using OpenMacroBoard.SDK;
using System;

namespace OpenMacroBoard.Examples.CommonStuff
{
    public static class DeckExtensions
    {
        public static int GetDeviceImageSize(this IMacroBoard deck)
        {
            if (!(deck.Keys is GridKeyPositionCollection gridKeys))
                throw new NotSupportedException("Device is not supported");

            if (gridKeys.KeyWidth != gridKeys.KeyHeight)
                throw new NotSupportedException("Device is not supported");

            return gridKeys.KeyWidth;
        }
    }
}
