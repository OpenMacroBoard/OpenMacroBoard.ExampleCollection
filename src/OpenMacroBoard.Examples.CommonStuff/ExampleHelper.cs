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
        {
            var devices = StreamDeck
                            .EnumerateDevices()
                            .Select(d =>
                            {
                                return new NamedDeviceReferenceHandle(d.Open, "");
                            })
                            .ToList();

            devices.Add(new VirtualDeviceHandle(Hardware.StreamDeck.Keys, "Virtual Stream Deck"));
            devices.Add(new VirtualDeviceHandle(Hardware.StreamDeckMini.Keys, "Virtual Stream Deck Mini"));

            return SelectBoard(devices).Open();
        }

        public static IDeviceReferenceHandle SelectBoard(IEnumerable<IDeviceReferenceHandle> devices)
        {
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
