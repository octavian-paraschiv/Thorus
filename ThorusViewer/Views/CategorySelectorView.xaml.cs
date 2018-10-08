using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using ThorusViewer.Models;
using ThorusCommon.Engine;

namespace ThorusViewer.Views
{
    public delegate void CategoryChangedHandler(string category);

    /// <summary>
    /// Interaction logic for CategorySelectorView.xaml
    /// </summary>
    public partial class CategorySelectorView : UserControl
    {
        public string Category { get; set; }

        public CategorySelectorView()
        {
            InitializeComponent();
            this.Loaded += CategorySelectorView_Loaded;

            SimulationData.SnapshotListChanged += new EventHandler(SimulationData_SnapshotListChanged);
        }

        void SimulationData_SnapshotListChanged(object sender, EventArgs e)
        {
            ActivateCategoryButtons();
        }

        void CategorySelectorView_Loaded(object sender, RoutedEventArgs e)
        {
            ActivateCategoryButtons();
        }

        bool _allowEvents = false;

        private void ActivateCategoryButtons()
        {
            try
            {
                _allowEvents = false;

                rbAvg.IsEnabled = SimulationData.DataCategoryExists("stats/AVG");
                rbMin.IsEnabled = SimulationData.DataCategoryExists("stats/MIN");
                rbMax.IsEnabled = SimulationData.DataCategoryExists("stats/MAX");
                rbRaw.IsEnabled = SimulationData.DataCategoryExists("");

                string selectedCategory = App.ControlPanelModel.SelectedCategory;
                if (selectedCategory == null)
                {
                    if (rbAvg.IsEnabled)
                    {
                        this.Category = "stats/AVG";
                        rbAvg.IsChecked = true;
                    }
                    else if (rbRaw.IsEnabled)
                    {
                        this.Category = "";
                        rbRaw.IsChecked = true;
                    }
                    else if (rbMin.IsEnabled)
                    {
                        this.Category = "stats/MIN";
                        rbMin.IsChecked = true;
                    }
                    else if (rbMax.IsEnabled)
                    {
                        this.Category = "stats/MAX";
                        rbMax.IsChecked = true;
                    }
                }
            }
            finally
            {
                _allowEvents = true;
            }
        }

        private void OnCategoryChange(object sender, RoutedEventArgs e)
        {
            //if (_allowEvents)
            {
                RadioButton rb = sender as RadioButton;
                if (rb != null)
                {
                    string tag = rb.Tag as string;
                    if (tag != null)
                    {
                        App.ControlPanelModel.SelectedCategory = tag;
                        SimulationData.LookupDataFiles(tag);
                    }
                }
            }
        }
    }
}
