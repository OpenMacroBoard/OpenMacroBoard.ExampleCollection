using OpenMacroBoard.SDK;
using System;

namespace OpenMacroBoard.Examples.CommonStuff
{
    public class CustomNameDeviceHandle : IDeviceReferenceHandle
    {
        private readonly string deviceName;
        private readonly IDeviceReferenceHandle handle;

        public CustomNameDeviceHandle(string deviceName, IDeviceReferenceHandle handle)
        {
            this.deviceName = deviceName ?? throw new ArgumentNullException(nameof(deviceName));
            this.handle = handle ?? throw new ArgumentNullException(nameof(handle));
        }

        public IMacroBoard Open()
            => handle.Open();

        public override string ToString()
            => deviceName;
    }
}
