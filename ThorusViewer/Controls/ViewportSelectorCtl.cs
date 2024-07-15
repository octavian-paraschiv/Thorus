using System;
using System.Windows.Forms;
using ThorusViewer.Models;

namespace OPMedia.UI.Controls
{
    public partial class ViewportSelectorCtl : UserControl
    {
        public ViewportSelectorCtl()
        {
            InitializeComponent();

            cmbViewport.DataSource = ControlPanelModel.Instance.Viewports;
            cmbViewport.SelectedItem = ControlPanelModel.Instance.SelectedViewport;
            cmbViewport.SelectedIndexChanged += OnViewportChanged;
        }

        void OnViewportChanged(object sender, EventArgs e)
        {
            Viewport selViewport = cmbViewport.SelectedItem as Viewport;
            ControlPanelModel.Instance.SelectedViewport = selViewport;
        }
    }
}
