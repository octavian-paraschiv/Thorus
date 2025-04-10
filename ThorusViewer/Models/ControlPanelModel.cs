﻿using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using ThorusCommon.Engine;
using ThorusCommon.Export;

namespace ThorusViewer.Models
{
    public class ControlPanelModel : INotifyPropertyChanged
    {
        public static ControlPanelModel Instance { get; } = new ControlPanelModel();

        public event PropertyChangedEventHandler PropertyChanged;

        #region Viewport settings

        private Viewport _selViewport = null;
        public Viewport SelectedViewport
        {
            get
            {
                return _selViewport;
            }

            set
            {
                _selViewport = value;
                FirePropertyChanged("SelectedViewport");
            }
        }
        #endregion

        #region Date type settings
        public List<DataType> DataTypes { get; private set; }

        private DataType _selDataType = null;
        public DataType SelectedDataType
        {
            get
            {
                return _selDataType;
            }

            set
            {
                _selDataType = value;
                FirePropertyChanged("SelectedDataType");
            }
        }

        private bool _autoSaveImage = false;
        public bool AutoSaveImage
        {
            get
            {
                return _autoSaveImage;
            }

            set
            {
                _autoSaveImage = value;
                FirePropertyChanged("AutoSaveImage");
            }
        }

        private bool _autoSaveSubMatrix = false;
        public bool AutoSaveSubMatrix
        {
            get
            {
                return _autoSaveSubMatrix;
            }

            set
            {
                _autoSaveSubMatrix = value;
                FirePropertyChanged("AutoSaveSubMatrix");
            }
        }
        #endregion

        #region Snapshot settings

        private SimDateTime _selSnapshot = null;
        public SimDateTime SelectedSnapshot
        {
            get
            {
                return _selSnapshot;
            }

            set
            {
                _selSnapshot = value;
                FirePropertyChanged("SelectedSnapshot");
            }
        }
        #endregion

        #region Category settings

        private string _selCategory = null;
        public string SelectedCategory
        {
            get
            {
                return _selCategory;
            }

            set
            {
                _selCategory = value;
                FirePropertyChanged("SelectedCategory");
            }
        }
        #endregion

        public ControlPanelModel()
        {

            DataTypes = new List<DataType>();

            #region Surface data

            DataTypes.Add(new DataType("A_00", "Albedo (%)"));

            DataTypes.Add(new DataType("T_SL", "Night low temperature"));
            DataTypes.Add(new DataType("T_NL", "Night low temperature (normal values)"));
            DataTypes.Add(new DataType("T_DL", "Night low temp delta (actual to normal)"));

            DataTypes.Add(new DataType("T_SH", "Day high temperature"));
            DataTypes.Add(new DataType("T_NH", "Day high temperature (normal values)"));
            DataTypes.Add(new DataType("T_DH", "Day high temp delta (actual to normal)"));

            DataTypes.Add(new DataType("T_DA", "Average temp delta (actual to normal)"));

            DataTypes.Add(new DataType("T_TW", "Water temperature"));
            DataTypes.Add(new DataType("T_TL", "Soil temperature"));
            DataTypes.Add(new DataType("T_TS", "Combined surface temperature"));

            var meanTe = new DataType("T_TE", "Mean air temperature", false, true);
            DataTypes.Add(meanTe);

            DataTypes.Add(new DataType("N_DD", "Daily Snow"));
            DataTypes.Add(new DataType("R_DD", "Daily Rain"));

            DataTypes.Add(new DataType("R_00", "Soil Accumulated Rain"));
            DataTypes.Add(new DataType("N_00", "Total Snow Cover"));

            DataTypes.Add(new DataType("B_00", "Blizzard conditions severity"));


            DataTypes.Add(new DataType("C_00", "Precip Rate & Type"));
            DataTypes.Add(new DataType("L_00", "Lifted Index (Thunderstorm odds)"));

            #endregion

            #region Jet stream level
            DataTypes.Add(new DataType("D_XX", "Jet stream enforced deviation - Longitude"));
            DataTypes.Add(new DataType("D_YY", "Jet stream enforced deviation - Latitude"));
            #endregion


            #region Low Level data
            DataTypes.Add(new DataType("T_00", "Temperature @ low level (~50m)"));
            DataTypes.Add(new DataType("H_00", "Relative humidity @ low level (~50m)"));
            DataTypes.Add(new DataType("P_00", "Pressure @ low level  (~50m)"));
            DataTypes.Add(new DataType("P_00", "Windmap @ low level (~50m)", true));

            #endregion

            #region Mid level data
            DataTypes.Add(new DataType("T_01", "Temperature @ mid level (~1500m)"));
            DataTypes.Add(new DataType("H_01", "Relative humidity @ mid level (~1500m)"));
            DataTypes.Add(new DataType("P_01", "Pressure @ mid level (~1500m)"));
            DataTypes.Add(new DataType("P_01", "Windmap @ mid level (~1500m)", true));

            #endregion

            #region Top level data
            DataTypes.Add(new DataType("T_02", "Temperature @ top level (~5500m)"));
            DataTypes.Add(new DataType("H_02", "Relative humidity @ top level (~5500m)"));
            DataTypes.Add(new DataType("P_02", "Pressure @ top level (~5500m)"));
            DataTypes.Add(new DataType("P_02", "Windmap @ top level (~5500m)", true));
            #endregion

            #region Jet level data
            DataTypes.Add(new DataType("P_03", "Ridge pattern @ jet level (~10000m)"));
            DataTypes.Add(new DataType("P_03", "Windmap @ jet level (~10000)", true));
            #endregion

            #region Custom data

            DataTypes.Add(new DataType("D_00", "Custom data (low level)"));
            DataTypes.Add(new DataType("D_01", "Custom data (mid level)"));
            DataTypes.Add(new DataType("D_02", "Custom data (top level)"));
            DataTypes.Add(new DataType("D_AA", "Custom data"));

            DataTypes.Add(new DataType("D_BP", "Blocking potential"));
            DataTypes.Add(new DataType("D_FP", "Cyclogenetic potential"));

            #endregion

            #region Others

            DataTypes.Add(new DataType("M_00", "Air mass type"));

            DataTypes.Add(new DataType("E_WL", "Water/land mask"));
            DataTypes.Add(new DataType("E_00", "Elevation data"));

            DataTypes.Add(new DataType("F_00", "Fronts"));

            DataTypes.Add(new DataType("F_SI", "Fog"));

            DataTypes.Add(new DataType("E_LR", "Environmental Lapse Rate"));


            #endregion

            SelectedViewport = Viewport.AllViewports.Where(v => v.Code == "RO").FirstOrDefault();
            SelectedDataType = meanTe;
        }

        public void FirePropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChangedEventArgs e = new PropertyChangedEventArgs(propertyName);
                PropertyChanged(this, e);
            }
        }

        internal void DoAutoSave()
        {
            if (this.AutoSaveImage)
                FirePropertyChanged("AutoSave");
        }
    }

    public class DataType
    {
        public string Name { get; private set; }
        public string Comments { get; set; }
        public string Value { get; private set; }
        public bool IsWindMap { get; private set; }
        public bool IsStatsAvailable { get; private set; }

        public override string ToString()
        {
            if (IsWindMap)
                return string.Format("Windmap({0}) [{1}]", Name, Value);

            return string.Format("{0} [{1}]", Name, Value);
        }

        public DataType(string name, string value, bool isWindMap = false, bool isStatsAvailable = true)
        {
            this.Name = name;
            this.Value = value;
            this.IsWindMap = isWindMap;
            this.IsStatsAvailable = isStatsAvailable;
        }
    }
}
