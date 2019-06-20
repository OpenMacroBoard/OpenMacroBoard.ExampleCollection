using OpenMacroBoard.SDK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace OpenMacroBoard.Examples.CommonStuff
{
    public partial class BoardSelectorWindow : Form
    {
        public IDeviceReferenceHandle SelectedDevice
            => (IDeviceReferenceHandle)boardList.SelectedItem;

        public BoardSelectorWindow(IEnumerable<IDeviceReferenceHandle> devices)
        {
            InitializeComponent();

            boardList.Items.AddRange(devices.ToArray());
            UpdateButtonEnabled();
        }

        private void OK_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void UpdateButtonEnabled()
        {
            button1.Enabled = boardList.SelectedItem != null;
        }

        private void BoardList_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateButtonEnabled();
        }
    }
}
