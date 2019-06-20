using OpenMacroBoard.SDK;
using OpenMacroBoard.VirtualBoard;
using StreamDeckSharp;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace OpenMacroBoard.Examples.CommonStuff
{
    public static class ExampleHelper
    {
        private static readonly IUsbHidHardware[] virtualFallback = new[]
        {
            Hardware.StreamDeck,
            Hardware.StreamDeckMini,
            Hardware.StreamDeckXL
        };

        static ExampleHelper()
        {
            Application.EnableVisualStyles();
        }

        /// <summary>
        /// Searches for a real classic stream deck or creates a virtual one.
        /// All examples are designed for the 5x3 classic StreamDeck.
        /// </summary>
        /// <returns></returns>
        public static IMacroBoard OpenBoard()
            => OpenStreamDeckHardware();

        public static IMacroBoard OpenStreamDeck()
            => OpenStreamDeckHardware(Hardware.StreamDeck);

        public static IMacroBoard OpenStreamDeckMini()
            => OpenStreamDeckHardware(Hardware.StreamDeckMini);

        public static IMacroBoard OpenStreamDeckXL()
            => OpenStreamDeckHardware(Hardware.StreamDeckXL);

        private static IMacroBoard OpenStreamDeckHardware(params IUsbHidHardware[] allowedHardware)
            => SelectBoard(GetRealAndSimulatedDevices(allowedHardware)).Open();

        private static IEnumerable<IDeviceReferenceHandle> GetRealAndSimulatedDevices(params IUsbHidHardware[] allowedHardware)
        {
            var virtualHardware = allowedHardware;

            if (virtualHardware == null || virtualHardware.Length < 1)
                virtualHardware = virtualFallback;

            return StreamDeck
                    .EnumerateDevices(allowedHardware)
                    .Cast<IDeviceReferenceHandle>()
                    .Union(virtualHardware.Select(GetSimulatedDeviceForHardware));
        }

        private static VirtualDeviceHandle GetSimulatedDeviceForHardware(IHardware hardware)
            => new VirtualDeviceHandle(hardware.Keys, $"Virtual {hardware.DeviceName}");

        public static IDeviceReferenceHandle SelectBoard(IEnumerable<IDeviceReferenceHandle> devices)
        {
            var devList = devices.ToList();

            if (devList.Count < 1)
                return null;

            if (devList.Count == 1)
                return devList[0];

            var dialog = new BoardSelectorWindow(devices);
            dialog.ShowDialog();
            return dialog.SelectedDevice;
        }

        public static void WaitForKeyToExit()
        {
            Console.WriteLine("Please press any key (on PC keyboard) to exit this example.");
            Console.ReadKey();
        }
    }
}
