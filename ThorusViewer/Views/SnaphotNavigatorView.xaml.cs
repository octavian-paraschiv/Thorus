using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ThorusCommon.Engine;


namespace ThorusViewer.Views
{
    public delegate void SnapshotSelectedHandler(SimDateTime snaphot);

    /// <summary>
    /// Interaction logic for SnaphotNavigatorView.xaml
    /// </summary>
    public partial class SnaphotNavigatorView : UserControl
    {
        SimDateTime _crtSnapshot = null;
        SimDateTime _min = null;
        SimDateTime _max = null;

        public string Category { get; set; }

        public SnaphotNavigatorView()
        {
            this.Category = "stats/AVG";
            InitializeComponent();
            this.Loaded += SnaphotNavigatorView_Loaded;
            
        }

        void SnaphotNavigatorView_Loaded(object sender, RoutedEventArgs e)
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

                cmbSnapshots.ItemsSource = SimulationData.AvailableSnapshots;

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
                else if (cmbSnapshots.Items.Count > prevIndex)
                    cmbSnapshots.SelectedIndex = Math.Max(0, prevIndex);
                else
                    cmbSnapshots.SelectedIndex = 0;
            }
        }


        Timer _tmrDelayedStateChange = null;

        private void cmbSnapshots_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                _crtSnapshot = cmbSnapshots.SelectedItem as SimDateTime;
                App.ControlPanelModel.SelectedSnapshot = _crtSnapshot;
            }
            finally
            {
                if (_tmrDelayedStateChange == null)
                {
                    _tmrDelayedStateChange = new Timer();
                    _tmrDelayedStateChange.Interval = 500;
                    _tmrDelayedStateChange.AutoReset = true;
                    _tmrDelayedStateChange.Elapsed += _tmrDelayedClick_Elapsed;
                }

                _tmrDelayedStateChange.Start();
            }
        }

        
        void _tmrDelayedClick_Elapsed(object sender, ElapsedEventArgs e)
        {
            
            _tmrDelayedStateChange.Stop();
            Dispatcher.Invoke(new Action(ChangeButtonState));
        }

        private void ChangeButtonState()
        {
            ChangeButtonState(false);
        }

        private void ChangeButtonState(bool disableAll)
        {
            foreach (var x in LogicalTreeHelper.GetChildren(pnlLayout))
            {
                Button btn = x as Button;
                if (btn != null)
                {
                    if (disableAll)
                        btn.IsEnabled = false;
                    else
                    {
                        int offset = GetOffset(btn);
                        btn.IsEnabled = CanSelectSnapshot(offset);
                    }
                }
            }

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

        private void Button_Refresh(object sender, RoutedEventArgs e)
        {
            SimulationData.LookupDataFiles(null);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
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
            if (btn != null)
            {
                string tag = btn.Tag as string;
                if (tag != null)
                {
                    int offset = 0;
                    if (int.TryParse(tag, out offset))
                        return offset;
                }
            }

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
