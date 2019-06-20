using OpenMacroBoard.SDK;
using System;

namespace OpenMacroBoard.Examples.CommonStuff
{
    internal class NamedDeviceReferenceHandle : IDeviceReferenceHandle
    {
        private readonly Func<IMacroBoard> getMacroBoard;

        public NamedDeviceReferenceHandle(Func<IMacroBoard> getMacroBoard, string name)
        {
            this.getMacroBoard = getMacroBoard ?? throw new ArgumentNullException(nameof(getMacroBoard));
            Name = name ?? throw new ArgumentNullException(nameof(name));
        }

        public IDeviceReferenceHandle Handle { get; }
        public string Name { get; }

        public IMacroBoard Open()
            => getMacroBoard();

        public override string ToString()
            => Name;
    }
}
