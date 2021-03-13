using System;

namespace OpenMacroBoard.Examples.XorFlip
{
    public class ButtonPosEventsArgs : EventArgs
    {
        public ButtonPosEventsArgs(int x, int y)
        {
            X = x;
            Y = y;
        }

        public int X { get; }
        public int Y { get; }
    }
}
