using OpenMacroBoard.SDK;
using OpenMacroBoard.VirtualBoard;

namespace OpenMacroBoard.Examples.CommonStuff
{
    internal class VirtualDeviceHandle : NamedDeviceReferenceHandle
    {
        public VirtualDeviceHandle(IKeyPositionCollection keyLayout, string name)
            : base(() => BoardFactory.SpawnVirtualBoard(keyLayout), name)
        {
        }
    }
}
