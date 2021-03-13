using System;

namespace OpenMacroBoard.Examples.XorFlip
{
    public interface IXorFlipHost
    {
        int Width { get; }
        int Height { get; }

        event EventHandler<ButtonPosEventsArgs> ButtonPressed;
        event EventHandler ResetCommand;

        void GameWon();
        void DrawImages(XorFlipButtonState[,] buttonStates);
    }
}
