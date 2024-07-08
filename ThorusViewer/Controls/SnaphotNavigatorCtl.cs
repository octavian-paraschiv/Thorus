using System;
using System.Linq;
using System.Windows.Forms;
using ThorusCommon.Engine;
using ThorusViewer.Models;

namespace OPMedia.UI.Controls
{
    public partial class SnaphotNavigatorCtl : UserControl
    {
        SimDateTime _crtSnapshot = null;
        SimDateTime _min = null;
        SimDateTime _max = null;

        public string Category { get; set; }

        public SnaphotNavigatorCtl()
        {
            InitializeComponent();
            this.Category = "stats/AVG";
            this.Load += SnaphotNavigatorView_Loaded;
        }

        void SnaphotNavigatorView_Loaded(object sender, EventArgs e)
        {
            ChangeButtonState(true);
            LoadSnapshots();
        }

        void SimulationData_SnapshotListChanged(object sender, EventArgs e)
        {
            LoadSnapshots();
        }

        void LoadSnapshots()
        {
            SimDateTime prevSnapshot = _crtSnapshot;
            int prevIndex = cmbSnapshots.SelectedIndex;

            try
            {
                SimulationData.SnapshotListChanged -= SimulationData_SnapshotListChanged;

                cmbSnapshots.DataSource = SimulationData.AvailableSnapshots;

                if (SimulationData.AvailableSnapshots.Count > 0)
                {
                    _min = SimulationData.AvailableSnapshots[0];
                    _max = SimulationData.AvailableSnapshots[SimulationData.AvailableSnapshots.Count - 1];
                }
            }
            catch (Exception ex)
            {
                string s = ex.Message;
            }
            finally
            {
                SimulationData.SnapshotListChanged += SimulationData_SnapshotListChanged;

                if (prevSnapshot != null && SimulationData.AvailableSnapshots.Contains(prevSnapshot))
                    cmbSnapshots.SelectedItem = prevSnapshot;
                else if (cmbSnapshots.Items.Count > 0)
                {
                    if (cmbSnapshots.Items.Count > prevIndex)
                        cmbSnapshots.SelectedIndex = Math.Max(0, prevIndex);
                    else
                        cmbSnapshots.SelectedIndex = 0;
                }
            }
        }

        // Timer _tmrDelayedStateChange = null;

        private void OnSelectedSnapshotChanged(object sender, EventArgs e)
        {
            try
            {
                _crtSnapshot = cmbSnapshots.SelectedItem as SimDateTime;
                ControlPanelModel.Instance.SelectedSnapshot = _crtSnapshot;
            }
            finally
            {
                ChangeButtonState(false);

                //if (_tmrDelayedStateChange == null)
                //{
                //    _tmrDelayedStateChange = new Timer();
                //    _tmrDelayedStateChange.Interval = 500;
                //    _tmrDelayedStateChange.Tick += _tmrDelayedClick_Elapsed;
                //}

                //_tmrDelayedStateChange.Start();
            }
        }


        //void _tmrDelayedClick_Elapsed(object sender, EventArgs e)
        //{

        //    _tmrDelayedStateChange.Stop();
        //    ChangeButtonState(false);
        //}

        private void ChangeButtonState(bool disableAll)
        {
            pnlLayout.Controls.OfType<Button>().ToList().ForEach(btn =>
            {
                if (disableAll)
                    btn.Enabled = false;
                else
                {
                    int offset = GetOffset(btn);
                    btn.Enabled = CanSelectSnapshot(offset);
                }
            });
        }

        private bool CanSelectSnapshot(int offset)
        {
            if (offset == short.MinValue || offset == short.MaxValue)
                return true;

            if (_crtSnapshot != null && _min != null && _max != null)
            {
                try
                {
                    SimDateTime sdt = _crtSnapshot.AddHours(offset);
                    return (_min.GetHoursOffset(sdt) <= 0 && sdt.GetHoursOffset(_max) <= 0);
                }
                catch { }
            }

            return false;
        }

        private void Button_Refresh(object sender, EventArgs e)
            => SimulationData.LookupDataFiles(null);

        private void Button_Click(object sender, EventArgs e)
        {
            ChangeButtonState(true);
            Button _btnAction = sender as Button;
            string tag = _btnAction.Tag as string;
            if (tag != null)
            {
                int offset = GetOffset(_btnAction);
                SelectSnapshot(offset);
            }
        }

        private int GetOffset(Button btn)
        {
            if (btn?.Tag is string tag && int.TryParse(tag, out int offset))
                return offset;

            return 0;
        }

        private void SelectSnapshot(int offset)
        {
            if (offset != 0)
            {
                SimDateTime actualSdt = null;

                if (offset == short.MinValue)
                    actualSdt = _min;
                else if (offset == short.MaxValue)
                    actualSdt = _max;
                else if (offset == -3)
                    actualSdt = SimulationData.SelectPreviousSnapshot(_crtSnapshot);
                else if (offset == 3)
                    actualSdt = SimulationData.SelectNextSnapshot(_crtSnapshot);
                else
                {
                    SimDateTime sdt = _crtSnapshot.AddHours(offset);
                    actualSdt = SimulationData.SelectNearestSnapshot(sdt);
                }

                cmbSnapshots.SelectedItem = actualSdt;
            }
        }
    }
}
