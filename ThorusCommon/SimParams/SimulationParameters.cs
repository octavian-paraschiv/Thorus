﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Reflection;
using System.Text;
using ThorusCommon.Engine;
using ThorusCommon.IO;

namespace ThorusCommon
{
    public class SimulationParameters : IPrecipTypeBoundaries
    {
        const string DataFileName = "SimParams.thd";
        private readonly string DataFilePath = string.Empty;

        #region Singleton
        public static SimulationParameters __instance = new SimulationParameters();

        public static SimulationParameters Instance
        {
            get
            {
                return __instance;
            }
        }

        private SimulationParameters()
        {
            DataFilePath = Path.Combine(SimulationData.WorkFolder, DataFileName);
            if (File.Exists(DataFilePath) == false)
                File.Copy($"Data/{DataFileName}", DataFilePath);

            Init();
            LoadFromFile();
        }

        #endregion

        private static SimulationParameters FromString(string str)
        {
            SimulationParameters simp = new SimulationParameters();
            simp.LoadFromString(str);
            return simp;
        }

        public override string ToString()
        {
            return SaveToString();
        }


        private void Init()
        {
            this.DryLapseRate = 9.5f;
            this.HumidLapseRate = 6.5f;

            this.FrontsContribution = 0.55f;

            this.AirTempContribution = 0.8f;


            this.WaterTempChangeFactor = 0.05f;
            this.SoilTempChangeFactor = 0.182f;

            this.ContinentalPolarAirMassTemp = -5;
            this.TropicalContinentalAirMassTemp = 18;

            this._DefaultRefTemp = 25;

            this.MaxTeForSolidPrecip = -2.5f;
            this.MinTeForLiquidPrecip = 2.5f;
            this.MaxTsForFreezing = -0.1f;
            this.MinTsForMelting = 0.1f;
            this.MaxFreezingRainDelta = 5f;

            this.FrontsDelta = 0.2f;
        }


        #region Property persistence

        public void LoadFromFile()
        {
            if (File.Exists(DataFilePath))
            {
                string s = File.ReadAllText(DataFilePath);
                if (string.IsNullOrEmpty(s) == false)
                    LoadFromString(s);
            }
        }

        public void SaveToFile()
        {
            string s = this.ToString();
            File.WriteAllText(DataFilePath, s);
        }

        public SimulationParameters Clone()
        {
            string s = this.ToString();
            return FromString(s);

        }

        public void LoadFromString(string str)
        {
            Dictionary<string, string> nameValuePairs = new Dictionary<string, string>();

            using (MemoryStream ms = new MemoryStream(Encoding.ASCII.GetBytes(str)))
            using (StreamReader sr = new StreamReader(ms))
            {
                while (sr.EndOfStream == false)
                {
                    string line = sr.ReadLine();
                    string[] nameValuePair = line.Split("=".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                    if (nameValuePair.Length == 2)
                    {
                        string key = nameValuePair[0];
                        string val = nameValuePair[1];

                        if (nameValuePairs.ContainsKey(key) == false)
                            nameValuePairs.Add(key, val);
                        else
                            nameValuePairs[key] = val;
                    }
                }
            }

            var propArray = this.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
            if (propArray != null)
            {
                foreach (PropertyInfo p in propArray)
                {
                    if (p.CanWrite)
                    {
                        string propName = p.Name;
                        if (nameValuePairs.ContainsKey(propName))
                        {
                            string valStr = nameValuePairs[propName];
                            object value = null;
                            if (valStr != "<null>")
                            {
                                Type t = p.PropertyType.UnderlyingSystemType;
                                if (t.IsEnum)
                                {
                                    value = Enum.Parse(p.PropertyType.UnderlyingSystemType, valStr);
                                }
                                else
                                {
                                    value = Convert.ChangeType(valStr, p.PropertyType.UnderlyingSystemType);
                                }
                            }

                            p.SetValue(this, value, null);
                        }
                    }
                }
            }
        }

        private string SaveToString()
        {
            StringBuilder sb = new StringBuilder();

            var propArray = this.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
            if (propArray != null)
            {
                foreach (PropertyInfo p in propArray)
                {
                    if (p.CanRead && p.CanWrite)
                    {
                        var val = p.GetValue(this, null);
                        var nameValuePair = "";

                        if (val == null)
                            nameValuePair = string.Format("{0}=<null>", p.Name);
                        else
                            nameValuePair = string.Format("{0}={1}", p.Name, val);

                        sb.AppendLine(nameValuePair);
                    }
                }
            }

            return sb.ToString();
        }

        #endregion

        #region Atmosphere / Advection

        [Category("Atmosphere model / Cyclogenesys")]
        [Description("How prone is the atmosphere to generate cyclones")]
        [Range(0f, 1f)]
        [DefaultValue(0.33f)]
        public float CyclogeneticFactor { get; set; }

        [Category("Atmosphere model / Cyclogenesys")]
        [Description("How prone is the atmosphere to generate anticyclones")]
        [Range(0f, 1f)]
        [DefaultValue(0.33f)]
        public float AntiCyclogeneticFactor { get; set; }

        #endregion

        #region Temperature models

        [Category("Temperature model / Lapse rate")]
        [Description("This factor describes how does the temperature vary with altitude, in dry air conditions.")]
        [Range(8, 12)]
        [DefaultValue(9.5f)]
        public float DryLapseRate { get; set; }

        [Category("Temperature model / Lapse rate")]
        [Description("This factor describes how does the temperature vary with altitude, in humid unsaturated air conditions.")]
        [Range(8, 12)]
        [DefaultValue(6.5f)]
        public float HumidLapseRate { get; set; }


        [Category("Temperature model / Air")]
        [Description("Reference air temp at 2000m at 25 deg north, in spring and autumn.")]
        [DefaultValue(22.0f)]
        [Range(20, 30)]
        public float _DefaultRefTemp { get; set; }


        [Category("Temperature model / Air")]
        [Description("High Boundary temperature for Arctic/Antarctic (cA) air masses.")]
        [DefaultValue(-12f)]
        public float ArcticAirMassTemp { get; set; }

        [Category("Temperature model / Air")]
        [Description("High Boundary temperature for Polar Continental (cP) air masses.")]
        [DefaultValue(-5.0f)]
        public float ContinentalPolarAirMassTemp { get; set; }

        [Category("Temperature model / Air")]
        [Description("Boundary temperature for Polar Maritime (mP) air masses.")]
        [DefaultValue(5.0f)]
        public float MaritimePolarAirMassTemp { get; set; }

        [Category("Temperature model / Air")]
        [Description("Low Boundary temperature for Tropical Maritime (mT) air masses.")]
        [DefaultValue(14f)]
        public float MaritimeTropicalAirMassTemp { get; set; }

        [Category("Temperature model / Air")]
        [Description("Low Boundary temperature for Tropical Continental (cT) air masses.")]
        [DefaultValue(18f)]
        public float TropicalContinentalAirMassTemp { get; set; }



        [Category("Temperature model / Equilibrium temperature")]
        [Description("The contribution of surface temperature in calculating the Equilibrium temperature. Calculated as 1-AirTempContribution.")]
        public float SurfaceTempContribution { get { return 1 - AirTempContribution; } }

        [Category("Temperature model / Equilibrium temperature")]
        [Description("The contribution of air temperature in calculating the Equilibrium temperature")]
        [DefaultValue(0.8f)]
        [Range(0.5, 1)]
        public float AirTempContribution { get; set; }


        [Category("Temperature model / Surface")]
        [Description("How much of the air temperature is daily transmitted to a water-like surface.")]
        [DefaultValue(0.05f)]
        [Range(0.01, 0.1)]
        public float WaterTempChangeFactor { get; set; }

        [Category("Temperature model / Surface")]
        [Description("How much of the air temperature is daily transmitted to a land-like surface.")]
        [DefaultValue(0.182f)]
        [Range(0.1, 0.2)]
        public float SoilTempChangeFactor { get; set; }

        [Category("Temperature model / Ref Temp Delay")]
        [Description("Ref Temp Delay (days)")]
        [DefaultValue(-30)]
        [Range(-60, 60)]
        public int Delay { get; set; }

        #endregion

        #region Precipitation Model

        [Category("Precipitation model")]
        [Description("How much of the total precipitation is caused by frontal perturbations")]
        [DefaultValue(0.55f)]
        [Range(0.1, 1)]
        public float FrontsContribution { get; set; }

        [Category("Precipitation model")]
        [Description("How much of the total precipitation is caused by baric gradient influence. Calculated as 1-FrontsContribution.")]
        [DefaultValue(0.45f)]
        [Range(0.1, 1)]
        public float BaricGradientContribution { get { return 1 - FrontsContribution; } }


        [Category("Precipitation model")]
        [Description("Maximum allowed Equilibrium Temperature (Te) to have precipitation in solid phase before they hit the soil.")]
        [DefaultValue(-2.5f)]
        [Range(-10f, 10f)]
        public float MaxTeForSolidPrecip { get; set; }

        [Category("Precipitation model")]
        [Description("Minimum allowed Equilibrium Temperature (Te) to have precipitation in liquid phase before they hit the soil.")]
        [DefaultValue(2.5f)]
        [Range(-10f, 10f)]
        public float MinTeForLiquidPrecip { get; set; }


        [Category("Precipitation model")]
        [Description("Maximum allowed Sfc Temperature (Ts) to have liquid phase precipitation that are freezing when hitting the soil")]
        [DefaultValue(-0.1f)]
        [Range(-10f, 10f)]
        public float MaxTsForFreezing { get; set; }

        [Category("Precipitation model")]
        [Description("Minimum allowed Sfc Temperature (Ts) to have solid phase precipitation that are melting when hitting the soil")]
        [DefaultValue(0.1f)]
        [Range(-10f, 10f)]
        public float MinTsForMelting { get; set; }


        [Category("Precipitation model")]
        [Description("Maximum allored temperature difference between mid level and near-sfc level, so that solid phase precip are not melting on their way down.")]
        [DefaultValue(5f)]
        [Range(-10f, 10f)]
        public float MaxFreezingRainDelta { get; set; }

        #endregion

        #region Atmosphere / Jet Stream and Fronts

        [Category(" Atmosphere model / Jet Stream")]
        [Description("Mid-Level Temperature minimum absolute difference for a front")]
        [DefaultValue(0.2f)]
        public float FrontsDelta { get; set; }

        #endregion
    }
}
